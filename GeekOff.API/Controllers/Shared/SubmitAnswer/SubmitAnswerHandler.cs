namespace GeekOff.Handlers;

public class SubmitAnswerHandler
{
    public record Request : IRequest<ApiResponse<StringReturn>>
    {
        public required string YEvent { get; init; } = string.Empty;
        public required int RoundNum { get; init; }
        public required string TextAnswer { get; init; } = string.Empty;
        public required int QuestionNum { get; init; }
        public required int TeamNum { get; init; }
    }

    public class Handler(ContextGo contextGo, ILogger<SubmitAnswerHandler> logger) : IRequestHandler<Request, ApiResponse<StringReturn>>
    {
        private readonly ContextGo _contextGo = contextGo;
        private readonly ILogger<SubmitAnswerHandler> _logger = logger;

        public async Task<ApiResponse<StringReturn>> Handle(Request request, CancellationToken token)
        {
            var returnString = new StringReturn();

            // this condition should not be possible with MediaTr.
            if (string.IsNullOrEmpty(request.YEvent))
            {
                returnString.Message = "No event was specified.";
                return ApiResponse<StringReturn>.BadRequest(returnString);
            }

            if (request.RoundNum is < 1 or > 3)
            {
                returnString.Message = "Incorrect round number.";
                return ApiResponse<StringReturn>.BadRequest(returnString);
            }

            if (request.QuestionNum is < 1 or > 99 && request.RoundNum == 1)
            {
                returnString.Message = $"Bad Question - Question {request.QuestionNum} Team ID {request.TeamNum} YEvent {request.YEvent}";
                _logger.LogDebug(returnString.Message);
                _contextGo.LogError.Add(new LogError(){ErrorMessage = returnString.Message});
                await _contextGo.SaveChangesAsync(cancellationToken: token);
                return ApiResponse<StringReturn>.BadRequest(returnString);
            }

            if (request.QuestionNum is < 201 or > 299 && request.RoundNum == 2)
            {
                returnString.Message = $"Bad Question - Question {request.QuestionNum} Team ID {request.TeamNum} YEvent {request.YEvent}";
                _logger.LogDebug(returnString.Message);
                _contextGo.LogError.Add(new LogError(){ErrorMessage = returnString.Message});
                await _contextGo.SaveChangesAsync(cancellationToken: token);
                return ApiResponse<StringReturn>.BadRequest(returnString);
            }

            if (request.QuestionNum is < 301 or > 399 && request.RoundNum == 3)
            {
                returnString.Message = $"Bad Question - Question {request.QuestionNum} Team ID {request.TeamNum} YEvent {request.YEvent}";
                _logger.LogDebug(returnString.Message);
                _contextGo.LogError.Add(new LogError(){ErrorMessage = returnString.Message});
                await _contextGo.SaveChangesAsync(cancellationToken: token);
                return ApiResponse<StringReturn>.BadRequest(returnString);
            }

            if (request.TextAnswer is null or "")
            {
                returnString.Message = $"Null answer - Question {request.QuestionNum} Team ID {request.TeamNum} YEvent {request.YEvent}";
                _logger.LogDebug(returnString.Message);
                _contextGo.LogError.Add(new LogError(){ErrorMessage = returnString.Message});
                await _contextGo.SaveChangesAsync(cancellationToken: token);
                return ApiResponse<StringReturn>.BadRequest(returnString);
            }

            if (request.TeamNum <= 0)
            {
                returnString.Message = $"Zero team - Question {request.QuestionNum} Team ID {request.TeamNum} YEvent {request.YEvent}";
                _logger.LogDebug(returnString.Message);
                _contextGo.LogError.Add(new LogError(){ErrorMessage = returnString.Message});
                await _contextGo.SaveChangesAsync(cancellationToken: token);
                return ApiResponse<StringReturn>.BadRequest(returnString);
            }

            var existAnswer = await _contextGo.UserAnswer.Where(u =>
                u.QuestionNum == request.QuestionNum &&
                u.TeamNum == request.TeamNum &&
                u.RoundNum == request.RoundNum &&
                u.Yevent == request.YEvent)
                .ToListAsync(cancellationToken: token);

            if (existAnswer is not null)
            {
                _contextGo.UserAnswer.RemoveRange(existAnswer);
                await _contextGo.SaveChangesAsync(cancellationToken: token);
            }

            var newAnswer = new UserAnswer()
            {
                Yevent = request.YEvent,
                TeamNum = request.TeamNum,
                QuestionNum = request.QuestionNum,
                TextAnswer = request.TextAnswer,
                RoundNum = request.RoundNum,
                AnswerTime = DateTime.UtcNow,
                AnswerUser = ""
            };

            await _contextGo.AddAsync(newAnswer, cancellationToken: token);
            await _contextGo.SaveChangesAsync(cancellationToken: token);

            returnString.Message = "Your answer is in. Good luck!";
            return ApiResponse<StringReturn>.Success(returnString);
        }
    }
}
