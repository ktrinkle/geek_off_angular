namespace GeekOff.Handlers;

public class SetEventHandler
{
    public class Request : IRequest<ApiResponse<StringReturn>>
    {
        public string YEvent { get; set; } = string.Empty;
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<StringReturn>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<StringReturn>> Handle(Request request, CancellationToken token)
        {
            var returnString = new StringReturn();
            var eventExist = await _contextGo.EventMaster
                .SingleOrDefaultAsync(e => e.Yevent == request.YEvent, cancellationToken: token);

            if (eventExist is null)
            {
                returnString.Message = "The selected event does not exist.";
                return ApiResponse<StringReturn>.NotFound(returnString);
            }

            var eventUpdate = await _contextGo.EventMaster
                .SingleOrDefaultAsync(e => e.SelEvent ?? false, cancellationToken: token);

            if (eventUpdate is not null)
            {
                eventUpdate.SelEvent = false;
                _contextGo.EventMaster.Update(eventUpdate);
            }

            eventExist.SelEvent = true;
            _contextGo.EventMaster.Update(eventExist);

            _contextGo.SaveChanges();

            returnString.Message = "The selected event was made active.";
            return ApiResponse<StringReturn>.Success(returnString);

        }
    }
}
