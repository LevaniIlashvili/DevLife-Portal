using DevLifePortal.Application.Contracts.Application;
using Microsoft.AspNetCore.SignalR;

namespace DevLifePortal.Api.Hubs
{
    public class BugChaseHub : Hub
    {
        private readonly IBugChaseService _bugChaseService;

        public BugChaseHub(IBugChaseService bugChaseService)
        {
            _bugChaseService = bugChaseService;
        }

        public async Task SendScore(int score)
        {
            var httpContext = Context.GetHttpContext();

            var userId = httpContext.Session.GetString("UserId");

            await _bugChaseService.UpdateProfileScore(int.Parse(userId), score);

            var topProfiles = await _bugChaseService.GetTopProfiles();
            await Clients.All.SendAsync("LeaderboardUpdated", topProfiles);
        }

        public async override Task OnConnectedAsync()
        {
            var leaderboard = await _bugChaseService.GetTopProfiles();
            await Clients.Caller.SendAsync("LeaderboardUpdated", leaderboard);

            await base.OnConnectedAsync();
        }
    }
}
