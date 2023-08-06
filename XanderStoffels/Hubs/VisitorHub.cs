using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace XanderStoffels.Hubs;

public class VisitorHub : Hub<IWebsiteVisitor>
{
    private static ConcurrentDictionary<string, Mouse> _connectedMice = new();
    public static int CurrentVideo = 1;
    public static DateTime SongStartedAt = DateTime.UtcNow;
    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.ChangeVideo(CurrentVideo, SongStartedAt);
        await Clients.Others.SpawnMouse(Context.ConnectionId, "mouse.png");
        foreach (var mouse in _connectedMice.Values)
            await Clients.Caller.SpawnMouse(mouse.Id, mouse.Image);

        _connectedMice.TryAdd(Context.ConnectionId, new(Context.ConnectionId, "pointer"));
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _connectedMice.TryRemove(Context.ConnectionId, out _);
        await Clients.Others.DestroyMouse(Context.ConnectionId);
    }

    public Task MoveMouse(int x, int y)
    {
        return Clients.Others.MoveMouse(Context.ConnectionId, x, y);
    }
    
    public Task ReceiveTerminalInput(string input)
    {
        return Clients.All.ReceiveChatMessage(Context.ConnectionId, input, false);
    }

}

public record Mouse(string Id, string Image);
