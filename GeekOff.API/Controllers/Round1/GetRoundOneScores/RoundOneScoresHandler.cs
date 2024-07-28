using System.Net.Quic;

namespace GeekOff.Handlers;

public class RoundOneScoresHandler
{
    public record Request : IRequest<ApiResponse<List<Round1Scores>>>
    {
        public required string YEvent { get; set; } = string.Empty;
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<List<Round1Scores>>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<List<Round1Scores>>> Handle(Request request, CancellationToken token)
        {
            if (string.IsNullOrEmpty(request.YEvent))
            {
                return ApiResponse<List<Round1Scores>>.NotFound();
            }

            // question and team needs
            // originally this was a big Linq query but that didn't work in unit tests, so refactored to objects.

            var teamResponse = await _contextGo.Teamreference.AsNoTracking().Where(tr => tr.Yevent == request.YEvent)
                                .Select(tr => new Round1Scores()
                                {
                                    TeamName = tr.Teamname!.ToUpper(),
                                    TeamNum = tr.TeamNum,
                                    Bonus = tr.Dollarraised >= 200 ? 10 : tr.Dollarraised > 100 ? (int)(tr.Dollarraised - 100) / 10 : 0
                                }).OrderBy(tr => tr.TeamNum).ToListAsync(token);

            var innerQuestionList = await _contextGo.QuestionAns.AsNoTracking().Where(qa => qa.Yevent == request.YEvent && qa.RoundNum == 1)
                                .OrderBy(qa => qa.QuestionNum).ToListAsync(token);

            var innerAnswerList = await _contextGo.Scoring.AsNoTracking().Where(qa => qa.Yevent == request.YEvent && qa.RoundNum == 1)
                                .OrderBy(qa => qa.QuestionNum).ToListAsync(token);

            foreach (var team in teamResponse)
            {
                // initial array
                var questions = innerQuestionList.Select(s => new Round1ScoreDetail
                {
                    QuestionNum = s.QuestionNum
                }).ToList();

                foreach(var question in questions)
                {
                    var ptsVal = innerAnswerList.Find(s => s.QuestionNum == question.QuestionNum && s.TeamNum == team.TeamNum);
                    if (ptsVal is not null)
                    {
                        question.QuestionScore = ptsVal.PointAmt;
                    }
                }

                team.Q.AddRange(questions);

                team.TeamScore = team.Q.Sum(s => s.QuestionScore) + team.Bonus;
            }

            return ApiResponse<List<Round1Scores>>.Success(teamResponse);
        }
    }
}
