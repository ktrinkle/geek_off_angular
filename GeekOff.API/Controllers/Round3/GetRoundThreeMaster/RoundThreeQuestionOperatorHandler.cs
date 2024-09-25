namespace GeekOff.Handlers;

public class RoundThreeQuestionOperatorHandler
{
    public record Request : IRequest<ApiResponse<List<Round3QuestionDto>>>
    {
        public string YEvent { get; set; } = string.Empty;
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<List<Round3QuestionDto>>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<List<Round3QuestionDto>>> Handle(Request request, CancellationToken token)
        {
             if (string.IsNullOrEmpty(request.YEvent))
             {
                return ApiResponse<List<Round3QuestionDto>>.BadRequest();
             }

             var round3Questions = await _contextGo.Scoreposs.AsNoTracking().Where(s => s.Yevent == request.YEvent
                                                && s.RoundNum == 3 && s.QuestionNum < 350)
                                        .Select(s => new Round3QuestionDto()
                                        {
                                            QuestionNum = s.QuestionNum,
                                            SortOrder = (decimal)s.QuestionNum % 10,
                                            Score = s.Ptsposs,
                                            Disabled = false,
                                        }).ToListAsync(token);

            if (round3Questions.Count == 0)
            {
                return ApiResponse<List<Round3QuestionDto>>.NotFound();
            }

            var round3Return = round3Questions
                .OrderBy(s => s.QuestionNum).ThenBy(s => s.SortOrder).ToList();

            return ApiResponse<List<Round3QuestionDto>>.Success(round3Return);
        }
    }
}
