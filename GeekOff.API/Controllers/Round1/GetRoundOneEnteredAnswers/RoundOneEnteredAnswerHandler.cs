namespace GeekOff.Handlers;

public class RoundOneEnteredAnswersHandler
{
    public class Request : IRequest<ApiResponse<List<Round1EnteredAnswers>>>
    {
        public string YEvent { get; set; } = string.Empty;
        public int QuestionNum { get; set;}
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<List<Round1EnteredAnswers>>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<List<Round1EnteredAnswers>>> Handle(Request request, CancellationToken token)
        {
            var returnDto = new List<Round1EnteredAnswers>();

            var submittedAnswer = await _contextGo.UserAnswer
                                    .Where(u => u.RoundNum == 1 && u.QuestionNum == request.QuestionNum && u.Yevent == request.YEvent)
                                    .ToListAsync(cancellationToken: token);

            var scoredAnswer = await _contextGo.Scoring.Where(s => s.RoundNum == 1 && s.Yevent == request.YEvent && s.QuestionNum == request.QuestionNum)
                                .ToListAsync(cancellationToken: token);

            // early exit removed, we always want a success from this API.

            foreach (var answer in submittedAnswer)
            {
                var displayAnswer = new Round1EnteredAnswers()
                {
                    TeamNum = answer.TeamNum,
                    QuestionNum = request.QuestionNum,
                    TextAnswer = answer.TextAnswer ?? string.Empty,
                    AnswerStatus = scoredAnswer.Any(s => s.TeamNum == answer.TeamNum)
                };
                returnDto.Add(displayAnswer);
            }

            return ApiResponse<List<Round1EnteredAnswers>>.Success(returnDto);
        }
    }
}
