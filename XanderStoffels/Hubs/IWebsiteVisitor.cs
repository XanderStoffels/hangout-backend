namespace XanderStoffels.Hubs;

public interface IWebsiteVisitor
{
    // Mouse stuff
    Task SpawnMouse(string id, string image);
    Task ChangeMouseImage(string id, string image);
    Task MoveMouse(string id, int x, int y);
    Task DestroyMouse(string id);

    // Terminal
    Task ReceiveSystemMessage(string message, LogLevel level);
    Task ReceiveChatMessage(string sender, string message, bool isAdmin);
    
    // Phone
    Task ChangeVideo(int videoId, DateTime startAt);
}