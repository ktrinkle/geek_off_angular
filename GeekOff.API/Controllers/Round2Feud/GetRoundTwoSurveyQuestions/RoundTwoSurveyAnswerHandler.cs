namespace GeekOff.Handlers;

public class RoundTwoSurveyQuestionHandler
{
    public record Request : IRequest<ApiResponse<List<Round2SurveyList>>>
    {
        public string YEvent { get; set; } = string.Empty;
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<List<Round2SurveyList>>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<List<Round2SurveyList>>> Handle(Request request, CancellationToken token)
        {
            // get the question text. This is duplicated from another method and probably should be a new shared service.
            var surveyReturn = await _contextGo.QuestionAns.Where(q => q.Yevent == request.YEvent && q.RoundNum == 2)
                                        .Select(q => new Round2SurveyList()
                                        {
                                            QuestionNum = q.QuestionNum,
                                            QuestionText = q.TextQuestion ?? string.Empty
                                        })
                                        .ToListAsync(token);

            return surveyReturn.Count == 0
                ? ApiResponse<List<Round2SurveyList>>.NotFound()
                : ApiResponse<List<Round2SurveyList>>.Success(surveyReturn);
        }
    }
}
