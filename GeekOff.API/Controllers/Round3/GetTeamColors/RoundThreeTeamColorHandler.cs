namespace GeekOff.Handlers;

public class RoundThreeTeamColorHandler
{
    public class ColorMatrix
    {
        public int Rnk { get; set; }
        public string Color { get; set; } = string.Empty;
    }

    public record Request : IRequest<ApiResponse<List<Round3TeamList>>>
    {
        public string YEvent { get; set; } = string.Empty;
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<List<Round3TeamList>>>
    {
        private readonly ContextGo _contextGo = contextGo;

        private readonly List<ColorMatrix> _teamColors =
        [
            new ColorMatrix { Rnk = 1, Color = "B" },
            new ColorMatrix { Rnk = 2, Color = "G" },
            new ColorMatrix { Rnk = 3, Color = "R" }
        ];

        public async Task<ApiResponse<List<Round3TeamList>>> Handle(Request request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.YEvent))
            {
            return ApiResponse<List<Round3TeamList>>.BadRequest();
            }

            var round3Teams = await _contextGo.Roundresult
                                .Where(tr => tr.RoundNum == 2 && tr.Rnk < 4 && tr.Yevent == request.YEvent)
                                .OrderBy(tr => tr.Rnk)
                                .Select(tr => new {tr.TeamNum, tr.Rnk}).ToListAsync(cancellationToken);

            var round3Return = round3Teams.Join(_teamColors,
                                    r3 => r3.Rnk,
                                    tc => tc.Rnk,
                                    (r3, tc) => new Round3TeamList()
                                        {
                                            Rnk = tc.Rnk,
                                            TeamNum = r3.TeamNum,
                                            TeamColor = tc.Color
                                        }
                                    ).ToList();

            return round3Return.Count != 0 ? ApiResponse<List<Round3TeamList>>.Success(round3Return) : ApiResponse<List<Round3TeamList>>.NotFound();
        }
    }
}
