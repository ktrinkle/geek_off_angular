namespace GeekOff.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly ContextGo _contextGo;
        private readonly ILogger<QuestionService> _logger;
        public QuestionService(ILogger<QuestionService> logger, ContextGo context)
        {
            _logger = logger;
            _contextGo = context;
        }

        public async Task<List<Round1QuestionDto>> GetRound1QuestionListWithAnswers(string yEvent)
        {
            var questionList = await _contextGo.QuestionAns.Where(q => q.Yevent == yEvent
                                                                            && q.RoundNum == 1).ToListAsync();

            if (questionList is null)
            {
                return null;
            }

            var questionReturn = new List<Round1QuestionDto>();

            foreach (var question in questionList)
            {
                var currentQuestion = new Round1QuestionDto() {
                    QuestionNum = question.QuestionNum,
                    QuestionText = question.TextQuestion,
                    Answers = new List<Round1Answers>()
                };

                if (question.MultipleChoice == true)
                {
                    currentQuestion.Answers.Add(new Round1Answers()
                    {
                        AnswerId = 1,
                        Answer = question.TextAnswer,
                    });

                    currentQuestion.Answers.Add(new Round1Answers()
                    {
                        AnswerId = 2,
                        Answer = question.TextAnswer2,
                    });

                    currentQuestion.Answers.Add(new Round1Answers()
                    {
                        AnswerId = 3,
                        Answer = question.TextAnswer3,
                    });

                    currentQuestion.Answers.Add(new Round1Answers()
                    {
                        AnswerId = 4,
                        Answer = question.TextAnswer4,
                    });

                    currentQuestion.AnswerType = question.MatchQuestion == true ? QuestionAnswerType.Match : QuestionAnswerType.MultipleChoice;
                }

                if (question.MultipleChoice == false)
                {
                    currentQuestion.AnswerType = QuestionAnswerType.FreeText;
                }

                questionReturn.Add(currentQuestion);

            }

            return questionReturn;
        }

        public async Task<List<Round1QuestionControlDto>> GetAllRound1Questions(string yEvent)
        {
            var returnList = new List<Round1QuestionControlDto>();

            var questions = await _contextGo.QuestionAns.Where(q => q.Yevent == yEvent
                                                        && q.RoundNum == 1).ToListAsync();

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
                    QuestionText = question.TextQuestion,
                    AnswerType = answerType,
                    AnswerText = question.TextAnswer
                };

                returnList.Add(transformedQuestion);
            }

            return returnList;
        }

        public async Task<bool> SubmitRound1Answer(string yEvent, int questionId, string answerText, int teamNum)
        {
            // test values
            if (questionId is < 1 or > 99)
            {
                var errorMsg = $"Bad Question - Question {questionId} Team ID {teamNum} YEvent {yEvent}";
                _logger.LogDebug(errorMsg);
                _contextGo.LogError.Add(new LogError(){ErrorMessage = errorMsg});
                await _contextGo.SaveChangesAsync();
                return false;
            }

            if (answerText is null or "")
            {
                var errorMsg = $"Null answer - Question {questionId} Team ID {teamNum} YEvent {yEvent}";
                _logger.LogDebug(errorMsg);
                _contextGo.LogError.Add(new LogError(){ErrorMessage = errorMsg});
                await _contextGo.SaveChangesAsync();
                return false;
            }

            if (teamNum <= 0)
            {
                var errorMsg = $"Zero user - Question {questionId} Team ID {teamNum} YEvent {yEvent}";
                _logger.LogDebug(errorMsg);
                _contextGo.LogError.Add(new LogError(){ErrorMessage = errorMsg});
                await _contextGo.SaveChangesAsync();
                return false;
            }

            var existAnswer = await _contextGo.UserAnswer.Where(u => u.QuestionNum == questionId && u.TeamNum == teamNum && u.Yevent == yEvent).ToListAsync();
            if (existAnswer is not null)
            {
                _contextGo.UserAnswer.RemoveRange(existAnswer);
                await _contextGo.SaveChangesAsync();
            }

            var newAnswer = new UserAnswer()
            {
                Yevent = yEvent,
                TeamNum = teamNum,
                QuestionNum = questionId,
                TextAnswer = answerText,
                RoundNum = 1,
                AnswerTime = DateTime.UtcNow,
                AnswerUser = ""
            };

            await _contextGo.AddAsync(newAnswer);
            await _contextGo.SaveChangesAsync();

            return true;
        }
    }
}
