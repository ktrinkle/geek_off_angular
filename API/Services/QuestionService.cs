using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekOff.Data;
using GeekOff.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        public async Task<List<Round1QuestionDisplay>> GetRound1QuestionAsync(string yEvent)
        {
            var questionList = await _contextGo.QuestionAns.Where(q => q.Yevent == yEvent
                                                            && q.RoundNo == 1).ToListAsync();

            if (questionList is null)
            {
                return null;
            }

            var questionReturn = new List<Round1QuestionDisplay>();

            foreach (var question in questionList)
            {
                var qDisplay = new Round1QuestionDisplay()
                    {
                        QuestionNum = question.QuestionNo,
                        QuestionText = question.TextQuestion,
                        Answers = new List<Round1Answers>(),
                        MediaFile = question.MediaFile,
                        MediaType = question.MediaType,
                        CorrectAnswer = question.CorrectAnswer
                    };

                if (question.MultipleChoice == true)
                {
                    qDisplay.Answers.Add(new Round1Answers()
                    {
                        AnswerId = 1,
                        Answer = question.TextAnswer,
                    });

                    qDisplay.Answers.Add(new Round1Answers()
                    {
                        AnswerId = 2,
                        Answer = question.TextAnswer2,
                    });

                    qDisplay.Answers.Add(new Round1Answers()
                    {
                        AnswerId = 3,
                        Answer = question.TextAnswer3,
                    });

                    qDisplay.Answers.Add(new Round1Answers()
                    {
                        AnswerId = 4,
                        Answer = question.TextAnswer4,
                    });

                    qDisplay.AnswerType = question.MatchQuestion == true ? QuestionAnswerType.Match : QuestionAnswerType.MultipleChoice;
                }

                if (question.MultipleChoice == false)
                {
                    qDisplay.AnswerType = QuestionAnswerType.FreeText;
                }

                questionReturn.Add(qDisplay);

            }

            return questionReturn;
        }

        public async Task<Round1QuestionDto> GetRound1QuestionWithAnswer(string yEvent, int questionNo)
        {
            var question = await _contextGo.QuestionAns.SingleOrDefaultAsync(q => q.QuestionNo == questionNo
                                                                            && q.Yevent == yEvent
                                                                            && q.RoundNo == 1);

            if (question is null)
            {
                return null;
            }

            var questionReturn = new Round1QuestionDto()
            {
                QuestionNum = questionNo,
                QuestionText = question.TextQuestion,
                Answers = new List<Round1Answers>(),
                ExpireTime = DateTime.UtcNow.AddSeconds(60)
            };

            if (question.MultipleChoice == true)
            {
                questionReturn.Answers.Add(new Round1Answers()
                {
                    AnswerId = 1,
                    Answer = question.TextAnswer,
                });

                questionReturn.Answers.Add(new Round1Answers()
                {
                    AnswerId = 2,
                    Answer = question.TextAnswer2,
                });

                questionReturn.Answers.Add(new Round1Answers()
                {
                    AnswerId = 3,
                    Answer = question.TextAnswer3,
                });

                questionReturn.Answers.Add(new Round1Answers()
                {
                    AnswerId = 4,
                    Answer = question.TextAnswer4,
                });

                questionReturn.AnswerType = question.MatchQuestion == true ? QuestionAnswerType.Match : QuestionAnswerType.MultipleChoice;
            }

            if (question.MultipleChoice == false)
            {
                questionReturn.AnswerType = QuestionAnswerType.FreeText;
            }

            return questionReturn;
        }

        public async Task<List<Round1QuestionDto>> GetRound1QuestionListWithAnswers(string yEvent)
        {
            var questionList = await _contextGo.QuestionAns.Where(q => q.Yevent == yEvent
                                                                            && q.RoundNo == 1).ToListAsync();

            if (questionList is null)
            {
                return null;
            }

            var questionReturn = new List<Round1QuestionDto>();

            foreach (var question in questionList)
            {
                var currentQuestion = new Round1QuestionDto() {
                    QuestionNum = question.QuestionNo,
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
                                                        && q.RoundNo == 1).ToListAsync();

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
                    QuestionNum = question.QuestionNo,
                    QuestionText = question.TextQuestion,
                    AnswerType = answerType,
                    AnswerText = question.TextAnswer
                };

                returnList.Add(transformedQuestion);
            }

            return returnList;
        }

        public async Task<bool> SubmitRound1Answer(string yEvent, int questionId, string answerText, string answerUser)
        {
            // test values
            if (questionId is < 1 or > 99)
            {
                var errorMsg = $"Bad Question - Question {questionId} User ID {answerUser} YEvent {yEvent}";
                _logger.LogDebug(errorMsg);
                _contextGo.LogError.Add(new LogError(){ErrorMessage = errorMsg});
                await _contextGo.SaveChangesAsync();
                return false;
            }

            if (answerText is null or "")
            {
                var errorMsg = $"Null answer - Question {questionId} User ID {answerUser} YEvent {yEvent}";
                _logger.LogDebug(errorMsg);
                _contextGo.LogError.Add(new LogError(){ErrorMessage = errorMsg});
                await _contextGo.SaveChangesAsync();
                return false;
            }

            if (answerUser is null or "000000")
            {
                var errorMsg = $"Zero user - Question {questionId} User ID {answerUser} YEvent {yEvent}";
                _logger.LogDebug(errorMsg);
                _contextGo.LogError.Add(new LogError(){ErrorMessage = errorMsg});
                await _contextGo.SaveChangesAsync();
                return false;
            }

            // grab team info for this user. If this fails, we exit early.
            var playerInfo = await _contextGo.TeamUser.FirstOrDefaultAsync(u => u.Username == answerUser && u.Yevent == yEvent);

            if (playerInfo is null)
            {
                var errorMsg = $"No player info - Question {questionId} User ID {answerUser} YEvent {yEvent}";
                _logger.LogDebug(errorMsg);
                _contextGo.LogError.Add(new LogError(){ErrorMessage = errorMsg});
                await _contextGo.SaveChangesAsync();
                return false;
            }

            var existAnswer = await _contextGo.UserAnswer.Where(u => u.QuestionNo == questionId && u.TeamNo == playerInfo.TeamNo && u.Yevent == yEvent).ToListAsync();
            if (existAnswer is not null)
            {
                _contextGo.UserAnswer.RemoveRange(existAnswer);
                await _contextGo.SaveChangesAsync();
            }

            var newAnswer = new UserAnswer()
            {
                Yevent = yEvent,
                TeamNo = playerInfo.TeamNo,
                QuestionNo = questionId,
                TextAnswer = answerText,
                RoundNo = 1,
                AnswerTime = DateTime.UtcNow,
                AnswerUser = answerUser
            };

            await _contextGo.AddAsync(newAnswer);
            await _contextGo.SaveChangesAsync();

            return true;
        }
    }
}
