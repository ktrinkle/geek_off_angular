namespace GeekOff.Handlers;

public class RoundOnePlayerQAndAHandler
{
    public class Request : IRequest<ApiResponse<List<Round1QuestionDto>>>
    {
        public string YEvent { get; set; } = string.Empty;
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<List<Round1QuestionDto>>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<List<Round1QuestionDto>>> Handle(Request request, CancellationToken token)
        {
            var questionList = await _contextGo.QuestionAns.Where(q => q.Yevent == request.YEvent
                                    && q.RoundNum == 1).ToListAsync(cancellationToken: token);

            if (questionList.Count == 0)
            {
                return ApiResponse<List<Round1QuestionDto>>.NotFound();
            }

            var questionReturn = new List<Round1QuestionDto>();

            foreach (var question in questionList)
            {
                var currentQuestion = new Round1QuestionDto() {
                    QuestionNum = question.QuestionNum,
                    QuestionText = question.TextQuestion!,
                    Answers = []
                };

                if (question.MultipleChoice == true)
                {
                    currentQuestion.Answers.Add(new Round1Answers()
                    {
                        AnswerId = 1,
                        Answer = question.TextAnswer!,
                    });

                    currentQuestion.Answers.Add(new Round1Answers()
                    {
                        AnswerId = 2,
                        Answer = question.TextAnswer2!,
                    });

                    currentQuestion.Answers.Add(new Round1Answers()
                    {
                        AnswerId = 3,
                        Answer = question.TextAnswer3!,
                    });

                    currentQuestion.Answers.Add(new Round1Answers()
                    {
                        AnswerId = 4,
                        Answer = question.TextAnswer4!,
                    });

                    currentQuestion.AnswerType = question.MatchQuestion == true ? QuestionAnswerType.Match : QuestionAnswerType.MultipleChoice;
                }

                if (question.MultipleChoice == false)
                {
                    currentQuestion.AnswerType = QuestionAnswerType.FreeText;
                }

                questionReturn.Add(currentQuestion);

            }

            return ApiResponse<List<Round1QuestionDto>>.Success(questionReturn);
        }
    }
}
