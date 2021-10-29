using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeekOff.Models;

namespace GeekOff.Services
{
    public interface IQuestionService
    {
        Task<List<Round1QuestionDisplay>> GetRound1QuestionAsync(string yEvent);
        Task<Round1QuestionDto> GetRound1QuestionWithAnswer(string yEvent, int questionNo);
        Task<List<Round1QuestionControlDto>> GetAllRound1Questions(string yEvent);
        Task<bool> SubmitRound1Answer(string yEvent, int questionId, string answerText, string answerUser);
        Task<List<Round1QuestionDto>> GetRound1QuestionListWithAnswers(string yEvent);
    }
}
