using Microsoft.AspNetCore.SignalR;
using XanderStoffels.Hubs;

namespace XanderStoffels.Background;

public class VideoSwitcher : BackgroundService
{
    
    private readonly IServiceProvider _services;

    public VideoSwitcher(IServiceProvider services)
    {
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _services.CreateScope();
            var hubContext = _services.GetRequiredService<IHubContext<VisitorHub, IWebsiteVisitor>>();
            
            // Show static.
            VisitorHub.SongStartedAt = DateTime.UtcNow;
            VisitorHub.CurrentVideo = -1;
            await hubContext.Clients.All.ChangeVideo(VisitorHub.CurrentVideo, VisitorHub.SongStartedAt);
            
            // Wait for 3 seconds.
            await Task.Delay(3000, stoppingToken);

            // Generate a new video Id.
            VisitorHub.SongStartedAt = DateTime.UtcNow;
            VisitorHub.CurrentVideo = new Random().Next(0, 4);
            await hubContext.Clients.All.ChangeVideo(VisitorHub.CurrentVideo, VisitorHub.SongStartedAt);

            // Wait for between 5 and 10 seconds.
            await Task.Delay(new Random().Next(5000, 10000), stoppingToken);
        }
    }
}