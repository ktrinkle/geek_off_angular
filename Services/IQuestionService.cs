using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeekOff.Models;

namespace GeekOff.Services
{
    public interface IQuestionService
    {
        Task<Round1QuestionDto> GetRound1Question(string yEvent, int questionNo);
        Task<Round1QuestionDto> GetRound1QuestionWithAnswer(string yEvent, int questionNo);
        Task<bool> SubmitRound1Answer(string yEvent, int questionId, int teamNo, string answerText, string answerUser);
    }
}