namespace GeekOff.Handlers;

public class RoundOneQuestionHandler
{
    public class Request : IRequest<ApiResponse<List<Round1QuestionDisplay>>>
    {
        public string YEvent { get; set; } = string.Empty;
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<List<Round1QuestionDisplay>>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<List<Round1QuestionDisplay>>> Handle(Request request, CancellationToken token)
        {
            var questionList = await _contextGo.QuestionAns.Where(q => q.Yevent == request.YEvent
                                && q.RoundNum == 1).ToListAsync(cancellationToken: token);

            if (questionList.Count == 0)
            {
                return ApiResponse<List<Round1QuestionDisplay>>.NotFound();
            }

            var questionReturn = new List<Round1QuestionDisplay>();

            foreach (var question in questionList)
            {
                var qDisplay = new Round1QuestionDisplay()
                {
                    QuestionNum = question.QuestionNum,
                    QuestionText = question.TextQuestion!,
                    Answers = [],
                    MediaFile = question.MediaFile,
                    MediaType = question.MediaType,
                    CorrectAnswer = question.CorrectAnswer!
                };

                if (question.MultipleChoice is true)
                {
                    qDisplay.Answers.Add(new Round1Answers()
                    {
                        AnswerId = 1,
                        Answer = question.TextAnswer!,
                    });

                    qDisplay.Answers.Add(new Round1Answers()
                    {
                        AnswerId = 2,
                        Answer = question.TextAnswer2!,
                    });

                    qDisplay.Answers.Add(new Round1Answers()
                    {
                        AnswerId = 3,
                        Answer = question.TextAnswer3!,
                    });

                    qDisplay.Answers.Add(new Round1Answers()
                    {
                        AnswerId = 4,
                        Answer = question.TextAnswer4!,
                    });

                    qDisplay.AnswerType = question.MatchQuestion == true ? QuestionAnswerType.Match : QuestionAnswerType.MultipleChoice;
                }

                if (question.MultipleChoice is false)
                {
                    qDisplay.AnswerType = QuestionAnswerType.FreeText;
                }

                questionReturn.Add(qDisplay);

            }

            return ApiResponse<List<Round1QuestionDisplay>>.Success(questionReturn);
        }
    }
}
