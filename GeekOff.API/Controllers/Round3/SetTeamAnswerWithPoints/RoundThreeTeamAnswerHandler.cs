namespace GeekOff.Handlers;

public class RoundThreeTeamAnswerHandler
{
    public record Request : IRequest<ApiResponse<StringReturn>>
    {
        public List<Round3AnswerDto> Round3Answers { get; set; } = [];
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<StringReturn>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<StringReturn>> Handle(Request request, CancellationToken token)
        {
            var returnString = new StringReturn();

            if (request.Round3Answers.Count == 0)
            {
                returnString.Message = "No answers were submitted.";
                return ApiResponse<StringReturn>.BadRequest(returnString);
            }

            var dbAnswer = new List<Scoring>();

            foreach (var submitAnswer in request.Round3Answers)
            {
                if (submitAnswer.Score is not null and (>0 or <0) ) {
                    var scoreRecord = new Scoring()
                    {
                        Yevent = submitAnswer.YEvent,
                        TeamNum = submitAnswer.TeamNum,
                        RoundNum = 3,
                        QuestionNum = submitAnswer.QuestionNum,
                        PointAmt = submitAnswer.Score,
                        Updatetime = DateTime.UtcNow
                    };

                    dbAnswer.Add(scoreRecord);
                }

            }

            await _contextGo.Scoring.AddRangeAsync(dbAnswer, token);
            await _contextGo.SaveChangesAsync(token);

            returnString.Message = "Scores were added to the system.";

            return ApiResponse<StringReturn>.Success(returnString);
        }
    }
}
