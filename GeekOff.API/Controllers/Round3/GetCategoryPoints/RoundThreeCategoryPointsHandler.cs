namespace GeekOff.Handlers;

public class RoundThreeCategoryPointsHandler
{
    public record Request : IRequest<ApiResponse<List<RoundThreeCategoryPoints>>>
    {
        public string YEvent { get; set; } = string.Empty;
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<List<RoundThreeCategoryPoints>>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<List<RoundThreeCategoryPoints>>> Handle(Request request, CancellationToken cancellationToken)
        {
             if (string.IsNullOrEmpty(request.YEvent))
             {
                return ApiResponse<List<RoundThreeCategoryPoints>>.BadRequest();
             }

             var round3Categories = await _contextGo.Scoreposs
                                        .Where(c => c.Yevent == request.YEvent && c.RoundNum == 3 && c.QuestionNum < 350)
                                        .OrderBy(c => c.QuestionNum)
                                        .AsNoTracking()
                                        .Select(c => new RoundThreeCategoryPoints
                                        {
                                            Yevent = c.Yevent,
                                            QuestionNum = c.QuestionNum,
                                            Ptsposs = c.Ptsposs,
                                            Enabled = false
                                        })
                                        .ToListAsync(cancellationToken);

            return round3Categories.Count == 0
                ? ApiResponse<List<RoundThreeCategoryPoints>>.NotFound()
                : ApiResponse<List<RoundThreeCategoryPoints>>.Success(round3Categories);
        }
    }
}
