namespace GeekOff.Handlers;

public class ScoreRoundOneAnswerAutomaticHandler
{
    public class Request : IRequest<ApiResponse<StringReturn>>
    {
        public string YEvent { get; set; } = string.Empty;
        public int QuestionNum { get; set; }
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<StringReturn>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<StringReturn>> Handle(Request request, CancellationToken token)
        {
            var returnString = new StringReturn();

            if (string.IsNullOrEmpty(request.YEvent))
            {
                returnString.Message = "Invalid event.";
                return ApiResponse<StringReturn>.NotFound(returnString);
            }

            if (request.QuestionNum is < 1 or > 99)
            {
                returnString.Message = "Invalid question.";
                return ApiResponse<StringReturn>.NotFound(returnString);
            }

            var submittedAnswers = await _contextGo.UserAnswer
                                    .Where(u => u.QuestionNum == request.QuestionNum && u.Yevent == request.YEvent)
                                    .AsNoTracking().ToListAsync(cancellationToken: token);
            var questionInfo = await _contextGo.QuestionAns.AsNoTracking()
                                    .FirstOrDefaultAsync(u => u.QuestionNum == request.QuestionNum
                                        && u.Yevent == request.YEvent, cancellationToken: token);
            var pointRef = await _contextGo.Scoreposs.AsNoTracking()
                                    .FirstOrDefaultAsync(p => p.QuestionNum == request.QuestionNum, cancellationToken: token);

            if (questionInfo is null)
            {
                returnString.Message = "Unable to load question.";
                return ApiResponse<StringReturn>.NotFound(returnString);
            }

            var submittedTeams = submittedAnswers.Select(t => t.TeamNum).ToList();

            var correctAnswer = questionInfo.CorrectAnswer;
            var ptsPoss = pointRef is not null ? pointRef.Ptsposs : 0;
            var scoring = new List<Scoring>();

            // remove existing answers for the auto-score process
            var answersToRemove = await _contextGo.Scoring.Where(s => s.Yevent == request.YEvent
                                        && s.RoundNum == 1 && submittedTeams.Contains(s.TeamNum)
                                        && s.QuestionNum == request.QuestionNum).ToListAsync(cancellationToken: token);

            if (answersToRemove is not null)
            {
                _contextGo.Scoring.RemoveRange(answersToRemove);
                await _contextGo.SaveChangesAsync(cancellationToken: token);
            }

            foreach (var answer in submittedAnswers)
            {
                if (answer.TextAnswer!.Equals(correctAnswer, StringComparison.CurrentCultureIgnoreCase))
                {
                    var teamScore = new Scoring()
                    {
                        Yevent = answer.Yevent,
                        TeamNum = answer.TeamNum,
                        RoundNum = 1,
                        QuestionNum = answer.QuestionNum,
                        TeamAnswer = answer.TextAnswer,
                        PointAmt = ptsPoss,
                        Updatetime = answer.AnswerTime
                    };

                    scoring.Add(teamScore);
                }
            }

            await _contextGo.AddRangeAsync(scoring, cancellationToken: token);
            await _contextGo.SaveChangesAsync(cancellationToken: token);

            returnString.Message = "Auto-scoring complete.";
            return ApiResponse<StringReturn>.Success(returnString);
        }
    }
}
