using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace JiraWhAzure.Hubs
{
    public class JiraSignalRHub : Hub
    {
    }

    public interface IJiraHookService
    {
        Task BroadcastMessage(string url);
    }

    public class JiraHookService : IJiraHookService
    {
        private readonly IHubContext<JiraSignalRHub> _hubContext;

        public JiraHookService(IHubContext<JiraSignalRHub> hubContext)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        }

        public async Task BroadcastMessage(string url)
        {
            await _hubContext.Clients.All.SendAsync("BroadcastMessage", url);
        }
    }
}
