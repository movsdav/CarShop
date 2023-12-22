using CarShop.Models.Accounts;

namespace CarShop.Models.Chat;

public class ChatChannel
{
    public int Id { get; set; }

    public required string User1Id { get; set; }
    public AppUser? User1 { get; set; }

    public required string User2Id { get; set; }
    public AppUser? User2 { get; set; }

    public List<Message>? Messages { get; set; }
}
