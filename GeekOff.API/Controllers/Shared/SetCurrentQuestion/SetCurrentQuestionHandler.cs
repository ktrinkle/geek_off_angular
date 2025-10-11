namespace GeekOff.Handlers;

public class SetCurrentQuestionHandler
{
    public record Request : IRequest<ApiResponse<CurrentQuestionDto>>
    {
        public required string YEvent { get; init; } = string.Empty;
        public required int RoundNum { get; init; }
        public required int QuestionNum { get; init; }
        public required int Status { get; init; }
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<CurrentQuestionDto>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<CurrentQuestionDto>> Handle(Request request, CancellationToken token)
        {
            var returnDto = new CurrentQuestionDto();

            if (string.IsNullOrEmpty(request.YEvent))
            {
                return ApiResponse<CurrentQuestionDto>.NotFound(returnDto);
            }

            if (request.RoundNum is < 1 or > 3)
            {
                return ApiResponse<CurrentQuestionDto>.BadRequest(returnDto);
            }

            if (request.QuestionNum is < 1 or > 99 && request.RoundNum == 1)
            {
                return ApiResponse<CurrentQuestionDto>.BadRequest(returnDto);
            }

            if (request.QuestionNum is < 201 or > 299 && request.RoundNum == 2)
            {
                return ApiResponse<CurrentQuestionDto>.BadRequest(returnDto);
            }

            if (request.QuestionNum is < 301 or > 399 && request.RoundNum == 3)
            {
                return ApiResponse<CurrentQuestionDto>.BadRequest(returnDto);
            }

            if (request.Status is < 0 or > 3)
            {
                return ApiResponse<CurrentQuestionDto>.BadRequest(returnDto);
            }

            var newQuestionStatus = new CurrentQuestion()
            {
                YEvent = request.YEvent,
                QuestionNum = request.QuestionNum,
                Status = request.Status,
                QuestionTime = DateTime.UtcNow
            };

            _contextGo.CurrentQuestion.Add(newQuestionStatus);
            await _contextGo.SaveChangesAsync(token);

            returnDto.QuestionNum = request.QuestionNum;
            returnDto.Status = request.Status;

            return ApiResponse<CurrentQuestionDto>.Success(returnDto);
        }
    }
}
