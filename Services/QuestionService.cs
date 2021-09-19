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

        public async Task<Round1QuestionDto> GetRound1Question(string yEvent, int questionNo, int roundNo)
        {
            var question = await _contextGo.QuestionAns.SingleOrDefaultAsync(q => q.QuestionNo == questionNo 
                                                                            && q.Yevent == yEvent
                                                                            && q.RoundNo == roundNo);

            if (question is null)
            {
                return null;
            }

            var questionReturn = new Round1QuestionDto()
                                {
                                    QuestionNo = questionNo,
                                    QuestionText = question.TextAnswer
                                };
            
            return questionReturn;
        }

        public async Task<Round1QuestionDto> GetRound1QuestionWithAnswer(string yEvent, int questionNo, int roundNo)
        {
            var question = await _contextGo.QuestionAns.SingleOrDefaultAsync(q => q.QuestionNo == questionNo 
                                                                            && q.Yevent == yEvent
                                                                            && q.RoundNo == roundNo);

            if (question is null)
            {
                return null;
            }

            var questionReturn = new Round1QuestionDto()
                                {
                                    QuestionNo = questionNo,
                                    QuestionText = question.TextAnswer,
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
    }
}