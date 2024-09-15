namespace GeekOff.Handlers;

public class RoundTwoAnswerTextHandler
{
    public record Request : IRequest<ApiResponse<StringReturn>>
    {
        public string YEvent { get; init; }  = string.Empty;
        public int QuestionNum { get; init; }
        public int PlayerNum { get; init; }
        public int TeamNum { get; init; }
        public string Answer { get; init; }  = string.Empty;
        public int Score { get; init; }
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<StringReturn>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<StringReturn>> Handle(Request request, CancellationToken token)
        {
            var returnString = new StringReturn();
            // check limits
            if (string.IsNullOrEmpty(request.YEvent))
            {
                returnString.Message = "The event ID is not valid. Please try again.";
                return ApiResponse<StringReturn>.NotFound(returnString);
            }

            if (request.PlayerNum is not (> 0 and < 3))
            {
                returnString.Message = "The player number is not valid. Please try again.";
                return ApiResponse<StringReturn>.BadRequest(returnString);
            }

            if (request.QuestionNum is < 200 or > 299)
            {
                returnString.Message = "The question is not a valid round 2 question. Please try again.";
                return ApiResponse<StringReturn>.BadRequest(returnString);
            }

            if (request.TeamNum < 1)
            {
                returnString.Message = "A valid team number is required.";
                return ApiResponse<StringReturn>.BadRequest(returnString);
            }

            var scoreRecord = new Scoring()
            {
                Yevent = request.YEvent,
                TeamNum = request.TeamNum,
                RoundNum = 2,
                QuestionNum = request.QuestionNum,
                TeamAnswer = request.Answer,
                PlayerNum = request.PlayerNum,
                PointAmt = request.Score,
                Updatetime = DateTime.UtcNow
            };

            // check if score record exists. If so, nuke it.
            var recordExists = await _contextGo.Scoring.AnyAsync(s => s.Yevent == request.YEvent
                                                        && s.TeamNum == request.TeamNum
                                                        && s.QuestionNum == request.QuestionNum, token);

            if (recordExists)
            {
                _contextGo.Scoring.UpdateRange(scoreRecord);
                await _contextGo.SaveChangesAsync(token);
            }

            if (!recordExists)
            {
                await _contextGo.AddAsync(scoreRecord, token);
                await _contextGo.SaveChangesAsync(token);
            }

            returnString.Message =  "The answer was successfully saved.";
            return ApiResponse<StringReturn>.Success(returnString);
        }
    }
}
