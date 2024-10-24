using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;


namespace ClaimApp.Hubs
{
    public class ClaimStatusHub : Hub
    {
        public async Task UpdateClaimStatus(int claimId, string status)
        {
            // Send the update to all connected clients
            await Clients.All.SendAsync("ReceiveClaimStatusUpdate", claimId, status);
        }
    }
}
