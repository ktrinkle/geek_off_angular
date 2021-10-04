using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using GeekOff.Data;
using GeekOff.Models;

namespace GeekOff.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly contextGo _contextGo;
        private readonly ILogger<QuestionService> _logger;
        public QuestionService(ILogger<QuestionService> logger, contextGo context)
        {
            _logger = logger;
            _contextGo = context;
        }

        public async Task<Round1QuestionDto> GetRound1Question(string yEvent, int questionNo)
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
                                    QuestionText = question.TextQuestion
                                };
            
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

                questionReturn.AnswerType = question.CorrectAnswer.Length == 1 ? QuestionAnswerType.MultipleChoice : QuestionAnswerType.Match;
            }

            if (question.MultipleChoice == false)
            {
                questionReturn.AnswerType = QuestionAnswerType.FreeText;
            }
            
            return questionReturn;
        }

        public async Task<bool> SubmitRound1Answer(string yEvent, int questionId, int teamNo, string answerText, string answerUser)
        {
            // test values
            if (questionId < 100 || questionId > 199)
            {
                return false;
            }

            if (answerText is null || answerText == "")
            {
                return false;
            }

            var existAnswer = await _contextGo.UserAnswer.Where(u => u.QuestionNo == questionId && u.TeamNo == teamNo && u.Yevent == yEvent).ToListAsync();
            if (existAnswer is not null)
            {
                _contextGo.UserAnswer.RemoveRange(existAnswer);
                await _contextGo.SaveChangesAsync();
            }

            var newAnswer = new UserAnswer()
            {
                Yevent = yEvent,
                TeamNo = teamNo,
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