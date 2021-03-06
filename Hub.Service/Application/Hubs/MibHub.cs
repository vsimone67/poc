using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Hub.Service.Application.Hubs
{
    public class MibHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly ILogger<MibHub> _logger;
        public MibHub(ILogger<MibHub> logger)
        {
            _logger = logger;
        }
        public override async Task OnConnectedAsync()
        {
            _logger.LogDebug($"Client Connected {Context.User.Identity.Name}");
            await base.OnConnectedAsync();

            await this.Clients.All.SendAsync("UserConnected");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            await this.Clients.All.SendAsync("UserDisConnected");
        }
    }
}
