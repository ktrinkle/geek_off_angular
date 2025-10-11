using Microsoft.AspNetCore.Http.HttpResults;

namespace GeekOff.Handlers;

public class GetCurrentEventHandler
{
    public class Request : IRequest<string>
    {

    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, string>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<string> Handle(Request request, CancellationToken token)
        {
            var currentEvent = await _contextGo.EventMaster
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.SelEvent == true, cancellationToken: token);

            var returnString = string.Empty;

            if (currentEvent is not null)
            {
                returnString = currentEvent.Yevent;
            }

            return returnString;
        }
    }
}
