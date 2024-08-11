namespace GeekOff.Handlers;

public class GetQuestionHandler
{
    public class Request : IRequest<ApiResponse<List<Round13QuestionDisplay>>>
    {
        public string YEvent { get; set; } = string.Empty;
        public int RoundNum { get; set;}
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<List<Round13QuestionDisplay>>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<List<Round13QuestionDisplay>>> Handle(Request request, CancellationToken token)
        {
            if (string.IsNullOrEmpty(request.YEvent) || request.RoundNum < 1 || request.RoundNum > 3 )
            {
                return ApiResponse<List<Round13QuestionDisplay>>.BadRequest();
            }

            var questionList = await _contextGo.QuestionAns.Where(q => q.Yevent == request.YEvent
                                && q.RoundNum == request.RoundNum).ToListAsync(cancellationToken: token);

            if (questionList.Count == 0)
            {
                return ApiResponse<List<Round13QuestionDisplay>>.NotFound();
            }

            var questionReturn = new List<Round13QuestionDisplay>();

            foreach (var question in questionList)
            {
                var qDisplay = new Round13QuestionDisplay()
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

                if (question.MultipleChoice is false && request.RoundNum == 1)
                {
                    qDisplay.AnswerType = QuestionAnswerType.FreeText;
                }

                if (request.RoundNum == 3)
                {
                    qDisplay.AnswerType = QuestionAnswerType.Jeopardy;
                }

                questionReturn.Add(qDisplay);

            }

            return ApiResponse<List<Round13QuestionDisplay>>.Success(questionReturn);
        }
    }
}
