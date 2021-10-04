using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace GeekOff.Services  
{  
    public class EventHub: Hub
    {  
        public async Task SendMessage(string message)
        {
            Console.WriteLine("Sendmessage called");
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }  
}  