namespace GeekOff.Handlers;

public class GetCurrentEventHandler
{
    public class Request : IRequest<ApiResponse<StringReturn>>
    {

    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<StringReturn>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<StringReturn>> Handle(Request request, CancellationToken token)
        {
            var currentEvent = await _contextGo.EventMaster
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.SelEvent == true, cancellationToken: token);

            var returnString = new StringReturn();

            if (currentEvent is not null)
            {
                returnString.Message = currentEvent.Yevent;
            }

            return ApiResponse<StringReturn>.Success(returnString);
        }
    }
}
