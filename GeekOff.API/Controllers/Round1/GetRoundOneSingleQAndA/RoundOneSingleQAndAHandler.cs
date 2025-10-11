namespace GeekOff.Handlers;

public class RoundOneSingleQAndAHandler
{
    public class Request : IRequest<ApiResponse<Round1QuestionDto>>
    {
        public string YEvent { get; set; } = string.Empty;
        public int QuestionNum { get; set; }
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<Round1QuestionDto>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<Round1QuestionDto>> Handle(Request request, CancellationToken token)
        {
            var question = await _contextGo.QuestionAns.SingleOrDefaultAsync(q => q.Yevent == request.YEvent
                                && q.RoundNum == 1 && q.QuestionNum == request.QuestionNum, cancellationToken: token);

            if (question is null)
            {
                return ApiResponse<Round1QuestionDto>.NotFound();
            }

            var questionReturn = new Round1QuestionDto()
            {
                QuestionNum = question.QuestionNum,
                QuestionText = question.TextQuestion!,
                Answers = [],
                ExpireTime = DateTime.UtcNow.AddSeconds(60)
            };

            if (question.MultipleChoice is true)
            {
                questionReturn.Answers.Add(new Round1Answers()
                {
                    AnswerId = 1,
                    Answer = question.TextAnswer!,
                });

                questionReturn.Answers.Add(new Round1Answers()
                {
                    AnswerId = 2,
                    Answer = question.TextAnswer2!,
                });

                questionReturn.Answers.Add(new Round1Answers()
                {
                    AnswerId = 3,
                    Answer = question.TextAnswer3!,
                });

                questionReturn.Answers.Add(new Round1Answers()
                {
                    AnswerId = 4,
                    Answer = question.TextAnswer4!,
                });

                questionReturn.AnswerType = question.MatchQuestion == true ? QuestionAnswerType.Match : QuestionAnswerType.MultipleChoice;
            }

            if (question.MultipleChoice is false)
            {
                questionReturn.AnswerType = QuestionAnswerType.FreeText;
            }

            return ApiResponse<Round1QuestionDto>.Success(questionReturn);
        }
    }
}
