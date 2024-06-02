namespace GeekOff.Handlers;

public class UpdateFundAmtHandler
{
    public class Request : IRequest<ApiResponse<StringReturn>>
    {
        public string YEvent { get; set; } = string.Empty;
        public int TeamNum { get; init; }
        public decimal? DollarAmt { get; set; }
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<StringReturn>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<StringReturn>> Handle(Request request, CancellationToken token)
        {
            var returnString = new StringReturn();

            if (request.DollarAmt is not null and < 0)
            {
                returnString.Message = "You can't have a fundraising amount less than zero.";
                return ApiResponse<StringReturn>.BadRequest(returnString);
            }

            // validate team number
            var teamInfo = await _contextGo.Teamreference.FirstOrDefaultAsync(tr => tr.Yevent == request.YEvent
                                                                                && tr.TeamNum == request.TeamNum);

            if (teamInfo is null)
            {
                returnString.Message = "Invalid team number is entered.";
                return ApiResponse<StringReturn>.NotFound(returnString);
            }

            teamInfo.Dollarraised = request.DollarAmt;
            _contextGo.Teamreference.Update(teamInfo);
            await _contextGo.SaveChangesAsync();

            returnString.Message = "The dollar amount is successfully updated.";
            return ApiResponse<StringReturn>.Success(returnString);
        }
    }
}
