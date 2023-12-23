using CarShop.Data;
using CarShop.Models.Chat;
using Microsoft.AspNetCore.SignalR;

namespace CarShop.Hubs;

public class ChatHub : Hub
{
    private static Dictionary<string, string> userConnections = new();
    private readonly ApplicationDbContext _context;

    public ChatHub(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task JoinChat(string chatId)
    {
        userConnections[Context.ConnectionId] = chatId;

        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }

    public async Task SendMessage(Message msg)
    {
        _context.Messages.Add(msg);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch
        {
            throw new Exception();
        }

        await Clients.Group(msg.ChatChannelId.ToString()).SendAsync("ReceiveMessage", msg);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (userConnections.TryGetValue(Context.ConnectionId, out var chatId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
        }
        userConnections.Remove(Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }

    public class MsgType
    {
        public string Content { get; set; }
        public string ReceiverId { get; set; }
        public string SenderId { get; set; }
        public int ChatId { get; set; }
    }
}
