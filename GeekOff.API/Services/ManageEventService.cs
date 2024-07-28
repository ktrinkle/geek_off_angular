namespace GeekOff.Services
{
    public class ManageEventService(ILogger<ManageEventService> logger, ContextGo context) : IManageEventService
    {
        private readonly ContextGo _contextGo = context;
        private readonly ILogger<ManageEventService> _logger = logger;

        public async Task<List<Round2SurveyList>> GetRound2QuestionList(string yEvent)
        {
            // get the question text
            var surveyReturn = await _contextGo.QuestionAns.Where(q => q.Yevent == yEvent && q.RoundNum == 2)
                                        .Select(q => new Round2SurveyList()
                                        {
                                            QuestionNum = q.QuestionNum,
                                            QuestionText = q.TextQuestion
                                        })
                                        .ToListAsync();

            return surveyReturn;
        }

        public async Task<string> SetRound2AnswerText(Round2AnswerDto submitAnswer)
        {
            // check limits
            if (submitAnswer.PlayerNum is not (> 0 and < 3))
            {
                return "The player number is not valid. Please try again.";
            }

            if (submitAnswer.QuestionNum is < 200 or > 299)
            {
                return "The question is not a valid round 2 question. Please try again.";
            }

            if (submitAnswer.TeamNum < 1)
            {
                return "A valid team number is required.";
            }

            var scoreRecord = new Scoring()
            {
                Yevent = submitAnswer.YEvent,
                TeamNum = submitAnswer.TeamNum,
                RoundNum = 2,
                QuestionNum = submitAnswer.QuestionNum,
                TeamAnswer = submitAnswer.Answer,
                PlayerNum = submitAnswer.PlayerNum,
                PointAmt = submitAnswer.Score,
                Updatetime = DateTime.UtcNow
            };

            // check if score record exists. If so, nuke it.
            var recordExists = await _contextGo.Scoring.AnyAsync(s => s.Yevent == submitAnswer.YEvent
                                                        && s.TeamNum == submitAnswer.TeamNum
                                                        && s.QuestionNum == submitAnswer.QuestionNum);

            if (recordExists)
            {
                _contextGo.Scoring.UpdateRange(scoreRecord);
                await _contextGo.SaveChangesAsync();
            }

            if (!recordExists)
            {
                await _contextGo.AddAsync(scoreRecord);
                await _contextGo.SaveChangesAsync();
            }

            return "The answer was successfully saved.";
        }

        public async Task<string> SetRound2AnswerSurvey(Round2AnswerDto submitAnswer)
        {
            // check limits
            if (submitAnswer.PlayerNum is not (> 0 and < 3))
            {
                return "The player number is not valid. Please try again.";
            }

            if (submitAnswer.QuestionNum is < 200 or > 299)
            {
                return "The question is not a valid round 2 question. Please try again.";
            }

            if (submitAnswer.TeamNum < 1)
            {
                return "A valid team number is required.";
            }

            if (submitAnswer.AnswerNum is < 0 or > 99)
            {
                return "A valid answer number is required.";
            }

            var answerText = await _contextGo.Scoreposs.SingleOrDefaultAsync(q => q.Yevent == submitAnswer.YEvent && q.RoundNum == 2 && q.QuestionNum == submitAnswer.QuestionNum && q.SurveyOrder == submitAnswer.AnswerNum);
            if (answerText == null)
            {
                return "Invalid answer number.";
            }

            var scoreRecord = new Scoring()
            {
                Yevent = submitAnswer.YEvent,
                TeamNum = submitAnswer.TeamNum,
                RoundNum = 2,
                QuestionNum = submitAnswer.QuestionNum,
                TeamAnswer = answerText.QuestionAnswer[0..11],
                PlayerNum = submitAnswer.PlayerNum,
                PointAmt = answerText.Ptsposs,
                Updatetime = DateTime.UtcNow
            };

            var recordExists = await _contextGo.Scoring.AnyAsync(s => s.Yevent == submitAnswer.YEvent
                                                        && s.TeamNum == submitAnswer.TeamNum
                                                        && s.QuestionNum == submitAnswer.QuestionNum);

            if (recordExists)
            {
                _contextGo.Scoring.UpdateRange(scoreRecord);
                await _contextGo.SaveChangesAsync();
            }

            if (!recordExists)
            {
                await _contextGo.AddAsync(scoreRecord);
                await _contextGo.SaveChangesAsync();
            }

            return "The answer was successfully saved.";
        }

        public async Task<string> FinalizeRound(string yEvent, int roundNum)
        {
            if (yEvent is null)
            {
                return "No event was specified.";
            }

            if (roundNum is < 1 or > 3)
            {
                return "Incorrect round number.";
            }

            var totalPoints = await _contextGo.Scoring.Where(s => s.RoundNum == roundNum && s.Yevent == yEvent)
                                .GroupBy(s => s.TeamNum)
                                .Select(s => new Round2FinalDto()
                                {
                                    TeamNum = s.Key,
                                    FinalScore = s.Sum(s => s.PointAmt)
                                }).ToListAsync();

            // now we rank and store into the DB. First we remove anything that already exists.

            var scorestoRemove = await _contextGo.Roundresult.Where(r => r.Yevent == yEvent && r.RoundNum == roundNum).ToListAsync();

            if (scorestoRemove.Count > 0)
            {
                _contextGo.RemoveRange(scorestoRemove);
                await _contextGo.SaveChangesAsync();
            }

            // add the new records
            var scorestoAdd = (from s in totalPoints
                               orderby s.FinalScore descending
                               select new Roundresult()
                               {
                                   Yevent = yEvent,
                                   TeamNum = s.TeamNum,
                                   RoundNum = roundNum,
                                   Ptswithbonus = s.FinalScore,
                                   Rnk = (from r in totalPoints
                                          where r.FinalScore > s.FinalScore
                                          select r).Count() + 1
                               }).ToList();

            await _contextGo.Roundresult.AddRangeAsync(scorestoAdd);
            await _contextGo.SaveChangesAsync();


            return "Scores have been finalized in the system.";

        }

        public async Task<string> SetRound3Answer(List<Round3AnswerDto> round3Answers)
        {
            if (round3Answers is null)
            {
                return "No answers were submitted.";
            }

            var dbAnswer = new List<Scoring>();

            foreach (var submitAnswer in round3Answers)
            {
                if (submitAnswer.Score is not null and (>0 or <0) ) {
                    var scoreRecord = new Scoring()
                    {
                        Yevent = submitAnswer.YEvent,
                        TeamNum = submitAnswer.TeamNum,
                        RoundNum = 3,
                        QuestionNum = submitAnswer.QuestionNum,
                        PointAmt = submitAnswer.Score,
                        Updatetime = DateTime.UtcNow
                    };

                    dbAnswer.Add(scoreRecord);
                }

            }

            await _contextGo.Scoring.AddRangeAsync(dbAnswer);
            await _contextGo.SaveChangesAsync();

            return "Scores were added to the system.";
        }

        public async Task<List<Round3QuestionDto>> GetRound3Master(string yEvent)
        {
            var round3Questions = await _contextGo.Scoreposs.Where(s => s.Yevent == yEvent && s.RoundNum == 3 && s.QuestionNum < 350)
                .Select(s => new Round3QuestionDto()
                {
                    QuestionNum = s.QuestionNum,
                    SortOrder = (decimal)s.QuestionNum % 10,
                    Score = s.Ptsposs
                }).ToListAsync();

            var round3Return = round3Questions
                .OrderBy(s => s.QuestionNum).OrderBy(s => s.SortOrder).ToList();


            return round3Return;
        }

        public async Task<List<IntroDto>> GetRound3Teams(string yEvent)
        {
            var round3Teams = await (from rr in _contextGo.Roundresult
                                     join tr in _contextGo.Teamreference
                                     on new { rr.TeamNum, rr.Yevent } equals new { tr.TeamNum, tr.Yevent }
                                     where rr.RoundNum == 2
                                     && rr.Yevent == yEvent
                                     && rr.Rnk < 4
                                     orderby rr.Rnk
                                     select new IntroDto()
                                     {
                                         TeamName = tr.Teamname,
                                         TeamNum = tr.TeamNum
                                     }).ToListAsync();

            return round3Teams;
        }
    }
}
