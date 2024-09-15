namespace GeekOff.Handlers;

public class RoundTwoAnswerSurveyHandler
{
    public record Request : IRequest<ApiResponse<StringReturn>>
    {
        public string YEvent { get; init; }  = string.Empty;
        public int QuestionNum { get; init; }
        public int PlayerNum { get; init; }
        public int TeamNum { get; init; }
        public int AnswerNum { get; init; }
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

            if (request.AnswerNum is < 0 or > 99)
            {
                returnString.Message = "A valid answer number is required.";
                return ApiResponse<StringReturn>.BadRequest(returnString);
            }

            var answerText = await _contextGo.Scoreposs.AsNoTracking()
                                .SingleOrDefaultAsync(q => q.Yevent == request.YEvent
                                && q.RoundNum == 2 && q.QuestionNum == request.QuestionNum
                                && q.SurveyOrder == request.AnswerNum, token);

            if (answerText == null)
            {
                returnString.Message = "The submitted answer number does not exist.";
                return ApiResponse<StringReturn>.NotFound(returnString);
            }

            var scoreRecord = new Scoring()
            {
                Yevent = request.YEvent,
                TeamNum = request.TeamNum,
                RoundNum = 2,
                QuestionNum = request.QuestionNum,
                TeamAnswer = answerText.QuestionAnswer![0..11],
                PlayerNum = request.PlayerNum,
                PointAmt = answerText.Ptsposs,
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
