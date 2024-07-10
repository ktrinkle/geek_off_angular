namespace GeekOff.Handlers;

public class RoundOneAdminQAndAHandler
{
    public class Request : IRequest<ApiResponse<List<Round1QuestionControlDto>>>
    {
        public string YEvent { get; set; } = string.Empty;
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<List<Round1QuestionControlDto>>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<List<Round1QuestionControlDto>>> Handle(Request request, CancellationToken token)
        {
            var returnList = new List<Round1QuestionControlDto>();

            var questions = await _contextGo.QuestionAns.Where(q => q.Yevent == request.YEvent
                                                        && q.RoundNum == 1).ToListAsync(cancellationToken: token);

            if (questions.Count == 0)
            {
                return ApiResponse<List<Round1QuestionControlDto>>.NotFound();
            }

            foreach (var question in questions)
            {
                var answerType = new QuestionAnswerType();
                if (question.MultipleChoice == false)
                {
                    answerType = QuestionAnswerType.FreeText;
                }

                if (question.MatchQuestion == true)
                {
                    answerType = QuestionAnswerType.Match;
                }

                if (question.MultipleChoice == true)
                {
                    answerType = QuestionAnswerType.MultipleChoice;
                }

                var transformedQuestion = new Round1QuestionControlDto()
                {
                    QuestionNum = question.QuestionNum,
                    QuestionText = question.TextQuestion!,
                    AnswerType = answerType,
                    AnswerText = question.TextAnswer!
                };

                returnList.Add(transformedQuestion);
            }

            return ApiResponse<List<Round1QuestionControlDto>>.Success(returnList);
        }
    }
}
