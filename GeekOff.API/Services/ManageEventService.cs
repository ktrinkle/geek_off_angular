namespace GeekOff.Services
{
    public class ManageEventService(ILogger<ManageEventService> logger, ContextGo context) : IManageEventService
    {
        private readonly ContextGo _contextGo = context;
        private readonly ILogger<ManageEventService> _logger = logger;

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
