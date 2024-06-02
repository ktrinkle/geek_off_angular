namespace GeekOff.Handlers;

public class AddEventHandler
{
    public class Request : IRequest<ApiResponse<StringReturn>>
    {
        public EventMaster NewEvent { get; set; } = new EventMaster();
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<StringReturn>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<StringReturn>> Handle(Request request, CancellationToken token)
        {
            var eventExist = await _contextGo.EventMaster.AnyAsync(e => e.Yevent == request.NewEvent.Yevent, token);
            var returnString = new StringReturn();

            if (eventExist)
            {
                returnString.Message = "The created event already exists. Please create a new code.";
                return ApiResponse<StringReturn>.Conflict(returnString);
            }

            // deactivate existing event if selected active in passed json
            if (request.NewEvent.SelEvent ?? false)
            {
                var eventUpdate = await _contextGo.EventMaster.SingleOrDefaultAsync(e => e.SelEvent ?? false, cancellationToken: token);
                eventUpdate!.SelEvent = false;
                _contextGo.EventMaster.Update(eventUpdate);
                _contextGo.SaveChanges();
            }

            await _contextGo.EventMaster.AddAsync(request.NewEvent, token);
            _contextGo.SaveChanges();

            returnString.Message = "The new event was successfully added to the system.";
            return ApiResponse<StringReturn>.Success(returnString);
        }
    }
}
