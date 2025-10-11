namespace GeekOff.Handlers;

public class GetCurrentQuestionHandler
{
    public class Request : IRequest<ApiResponse<CurrentQuestionDto>>
    {
        public string YEvent { get; init; } = string.Empty;
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<CurrentQuestionDto>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<CurrentQuestionDto>> Handle(Request request, CancellationToken token)
        {
            var currentQuestion = await _contextGo.CurrentQuestion
                                        .AsNoTracking()
                                        .Where(q => q.YEvent == request.YEvent)
                                        .OrderByDescending(q => q.QuestionTime)
                                        .Select(q => new CurrentQuestionDto()
                                        {
                                            QuestionNum = q.QuestionNum,
                                            Status = q.Status
                                        }).FirstOrDefaultAsync(cancellationToken: token);

            return currentQuestion is null ?
                ApiResponse<CurrentQuestionDto>.NoContent() :
                ApiResponse<CurrentQuestionDto>.Success(currentQuestion);
        }
    }
}
