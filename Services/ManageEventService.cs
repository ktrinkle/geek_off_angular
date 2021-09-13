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
    public class ManageEventService : IManageEventService
    {
        private readonly contextGo _contextGo;
        private readonly ILogger<ManageEventService> _logger;
        public ManageEventService(ILogger<ManageEventService> logger, contextGo context)
        {
            _logger = logger;
            _contextGo = context;
        }

        public async Task<List<Round2SurveyList>> GetRound2SurveyMaster(string yEvent)
        {
            var surveyAnswer = await _contextGo.Scoreposs.Where(q => q.Yevent == yEvent && q.RoundNo == 2)
                                        .ToListAsync();

            // elegant way isn't working so this is more brute force
            var surveyReturn = _contextGo.Scoreposs.Where(q => q.Yevent == yEvent && q.RoundNo == 2)
                                        .GroupBy(s => s.QuestionNo)
                                        .Select(q => new Round2SurveyList()
                                            {
                                                QuestionNo = q.Key
                                            })
                                        .ToList();

            foreach (Round2SurveyList survey in surveyReturn)
            {
                survey.SurveyAnswers = surveyAnswer.FindAll(s => s.QuestionNo == survey.QuestionNo)
                                                    .Select(s => new Round2Answers()
                                                    {
                                                        QuestionNo = s.QuestionNo,
                                                        Answer = s.QuestionAnswer,
                                                        Score = (int)s.Ptsposs
                                                    }).ToList();
            }

            return surveyReturn;            
        }

        public async Task<string> SetRound2Answer(string yEvent, int questionNo, int teamNo, int playerNum, string answer, int score)
        {
            // check limits
            if (playerNum < 1 || playerNum > 3)
            {
                var err = "The player number is not valid. Please try again.";
                return err;
            }

            if (questionNo < 200 || questionNo > 299)
            {
                return "The question is not a valid round 2 question. Please try again.";
            }

            if (teamNo < 1)
            {
                return "A valid team number is required.";
            }

            var scoreRecord = new Scoring()
            {
                Yevent = yEvent,
                TeamNo = teamNo,
                RoundNo = 2,
                QuestionNo = questionNo,
                TeamAnswer = answer,
                PlayerNum = playerNum,
                PointAmt = score,
                Updatetime = DateTime.UtcNow
            };

            await _contextGo.AddAsync(scoreRecord);
            await _contextGo.SaveChangesAsync();

            return "The answer was successfully saved.";
        }

        public Task<string> FinalizeRound(string yEvent)
        {

        }
        /*
        WITH res AS (SELECT ts.yevent, ts.round_no, ts.team_no, ts.ptswithbonus, 
		rank() over (ORDER BY ts.ptswithbonus DESC ) AS rnk FROM 
		totalscore ts, event_name en WHERE ts.round_no =  $round  AND ts.yevent 
		 = en.yevent AND en.sel_event = 1) INSERT INTO roundresult
		SELECT res.yevent, res.round_no, res.team_no, res.ptswithbonus, res.rnk
		FROM res ON CONFLICT (yevent, round_no, team_no) DO UPDATE SET
		ptswithbonus = excluded.ptswithbonus, rnk = excluded.rnk";
        */
    }
}
