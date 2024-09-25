namespace GeekOff.Handlers;

public class RoundThreeCategoryHandler
{
    public record Request : IRequest<ApiResponse<List<RoundCategory>>>
    {
        public string YEvent { get; set; } = string.Empty;
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<List<RoundCategory>>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<List<RoundCategory>>> Handle(Request request, CancellationToken token)
        {
             if (string.IsNullOrEmpty(request.YEvent))
             {
                return ApiResponse<List<RoundCategory>>.BadRequest();
             }

             var round3Categories = await _contextGo.RoundCategories
                                        .Where(c => c.Yevent == request.YEvent && c.RoundNum == 3)
                                        .OrderBy(c => c.SubCategoryNum)
                                        .AsNoTracking()
                                        .ToListAsync(token);

            if (round3Categories.Count == 0)
            {
                return ApiResponse<List<RoundCategory>>.NotFound();
            }

            return ApiResponse<List<RoundCategory>>.Success(round3Categories);
        }
    }
}
