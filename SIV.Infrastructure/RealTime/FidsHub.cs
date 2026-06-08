using Microsoft.AspNetCore.SignalR;

namespace SIV.Infrastructure.RealTime
{
    public class FidsHub : Hub
    {
        public async Task UnirseAlTablero()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "PantallasFIDS");
        }
    }
}
