using CarShop.Models.Accounts;

namespace CarShop.Models.Chat;

public class Message
{
    public int Id { get; set; }
    public required string Content { get; set; }

    public required string? SenderId { get; set; }
    public AppUser? Sender { get; set; }


    public required string? ReceiverId { get; set; }
    public AppUser? Receiver { get; set; }

    public required int ChatChannelId { get; set; }
    public ChatChannel? ChatChannel { get; set; }
}
