namespace GeekOff.Handlers;

public class AddEventHandler
{
    public class Request : IRequest<ApiResponse<StringReturn>>
    {
        public string Yevent { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
        public bool SelEvent { get; set; } = false;
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<StringReturn>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<StringReturn>> Handle(Request request, CancellationToken cancellationToken)
        {
            var eventExist = await _contextGo.EventMaster.AnyAsync(e => e.Yevent == request.Yevent, cancellationToken);
            var returnString = new StringReturn();

            if (eventExist)
            {
                returnString.Message = "The created event already exists. Please create a new code.";
                return ApiResponse<StringReturn>.Conflict(returnString);
            }

            // deactivate existing event if selected active in passed json
            if (request.SelEvent)
            {
                var eventUpdate = await _contextGo.EventMaster.SingleOrDefaultAsync(e => e.SelEvent ?? false, cancellationToken);
                eventUpdate!.SelEvent = false;
                _contextGo.EventMaster.Update(eventUpdate);
                await _contextGo.SaveChangesAsync(cancellationToken);
            }

            var newEvent = new EventMaster()
            {
                Yevent = request.Yevent,
                SelEvent = request.SelEvent,
                EventName = request.EventName
            };

            await _contextGo.EventMaster.AddAsync(newEvent, cancellationToken);
            await _contextGo.SaveChangesAsync(cancellationToken);

            returnString.Message = "The new event was successfully added to the system.";
            return ApiResponse<StringReturn>.Success(returnString);
        }
    }
}
