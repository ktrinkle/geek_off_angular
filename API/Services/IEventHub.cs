using System.Threading.Tasks;

namespace GeekOff.Services
{
    public interface IEventHub
    {
        Task BroadcastMessage();
        Task SendMessageAsync(string message);
    }
}
