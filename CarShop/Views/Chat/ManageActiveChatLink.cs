namespace CarShop.Views.Chat;

public static class ManageActiveChatLink
{
    public static string? MakeActive(int currentChatId, int? activeChatId) =>
        currentChatId == activeChatId ? "active" : null;
}
