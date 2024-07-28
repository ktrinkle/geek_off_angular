namespace GeekOff.Handlers;

public class ScoreRoundOneAnswerManualHandler
{
    public record Request : IRequest<ApiResponse<StringReturn>>
    {
        public required string YEvent { get; init; } = string.Empty;
        public required int QuestionNum { get; init; }
        public required int TeamNum { get; init; }
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

            if (request.TeamNum <= 0 )
            {
                returnString.Message = "Invalid team number.";
                return ApiResponse<StringReturn>.NotFound(returnString);
            }

            // we don't need to look at the answer since a human has done so.
            // But we are flipping the score if it exists.

            var scoring = await _contextGo.Scoring.Where(s => s.Yevent == request.YEvent
                                    && s.TeamNum == request.TeamNum
                                    && s.QuestionNum == request.QuestionNum).ToListAsync(cancellationToken: token);

            if (scoring.Count > 0)
            {
                // remove the entry since it's a reversion
                _contextGo.Scoring.RemoveRange(scoring);
                await _contextGo.SaveChangesAsync(token);

                returnString.Message = "Removed the existing score for this team and question.";
                return ApiResponse<StringReturn>.Success(returnString);
            }

            if (scoring.Count == 0)
            {
                var teamAnswer = await _contextGo.UserAnswer.FirstOrDefaultAsync(
                                    u => u.Yevent == request.YEvent &&
                                    u.QuestionNum == request.QuestionNum &&
                                    u.TeamNum == request.TeamNum, token);

                var pointRef = await _contextGo.Scoreposs.AsNoTracking().FirstOrDefaultAsync(
                                    p => p.QuestionNum == request.QuestionNum, token);

                var ptsPoss = pointRef is not null ? pointRef.Ptsposs : 0;

                if (teamAnswer is not null)
                {
                    // add a new entry
                    var teamScore = new Scoring()
                    {
                        Yevent = request.YEvent,
                        TeamNum = request.TeamNum,
                        RoundNum = 1,
                        QuestionNum = request.QuestionNum,
                        TeamAnswer = teamAnswer.TextAnswer,
                        PointAmt = ptsPoss,
                        Updatetime = teamAnswer.AnswerTime
                    };

                    await _contextGo.Scoring.AddAsync(teamScore, token);
                    await _contextGo.SaveChangesAsync(token);

                    returnString.Message = "Scoring complete.";
                    return ApiResponse<StringReturn>.Success(returnString);
                }
            }

            // this should never be hit.
            returnString.Message = "We couldn't find an answer.";
            return ApiResponse<StringReturn>.BadRequest(returnString);
        }
    }
}
