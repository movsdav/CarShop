using CarShop.Models.Chat;

namespace CarShop.ViewModels;

public class ChatViewModel
{
    public List<ChatChannel>? ChatChannels { get; set; }
    public ChatChannel? ActiveChannel { get; set; }
}
