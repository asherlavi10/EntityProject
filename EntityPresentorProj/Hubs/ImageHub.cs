using Microsoft.AspNetCore.SignalR;

namespace EntityPresentorProj.Hubs
{
    public class ImageHub :Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

    }
    
}
