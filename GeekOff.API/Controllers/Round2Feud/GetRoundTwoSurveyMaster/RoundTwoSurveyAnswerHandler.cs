namespace GeekOff.Handlers;

public class RoundTwoSurveyAnswerHandler
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
            var surveyAnswer = await _contextGo.Scoreposs.Where(q => q.Yevent == request.YEvent && q.RoundNum == 2)
                                        .ToListAsync(token);

            if (surveyAnswer.Count == 0)
            {
                return ApiResponse<List<Round2SurveyList>>.NotFound();
            }

            // get the question text. This is duplicated from another method and probably should be a new shared service.
            var surveyReturn = await _contextGo.QuestionAns.Where(q => q.Yevent == request.YEvent && q.RoundNum == 2)
                                        .Select(q => new Round2SurveyList()
                                        {
                                            QuestionNum = q.QuestionNum,
                                            QuestionText = q.TextQuestion ?? string.Empty
                                        })
                                        .ToListAsync(token);

            foreach (var survey in surveyReturn)
            {
                survey.SurveyAnswers = surveyAnswer.FindAll(s => s.QuestionNum == survey.QuestionNum)
                                                    .Select(s => new Round2Answers()
                                                    {
                                                        QuestionNum = s.QuestionNum,
                                                        Answer = s.QuestionAnswer!,
                                                        Score = (int)s.Ptsposs!
                                                    }).ToList();
            }

            return ApiResponse<List<Round2SurveyList>>.Success(surveyReturn);
        }
    }
}
