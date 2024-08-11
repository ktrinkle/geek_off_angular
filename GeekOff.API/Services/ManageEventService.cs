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

    }
}
