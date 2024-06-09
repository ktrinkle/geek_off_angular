using System.Runtime.CompilerServices;

namespace GeekOff.Handlers;

public class EventListHandler
{
    public class Request : IRequest<ApiResponse<List<EventMaster>>>
    {

    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<List<EventMaster>>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<List<EventMaster>>> Handle(Request request, CancellationToken token)
        {
            var currentEvent = await _contextGo.EventMaster
                .AsNoTracking()
                .ToListAsync(cancellationToken: token);

            return currentEvent is null ? ApiResponse<List<EventMaster>>.Conflict() : ApiResponse<List<EventMaster>>.Success(currentEvent);
        }
    }
}
