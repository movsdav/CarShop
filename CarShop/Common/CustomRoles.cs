namespace CarShop.Common;

public static class CustomRoles
{
    public const string Admin = "Admin";
    public const string Dealer = "Dealer";
    public const string Client = "Client";

    public const string AdminOrDealer = $"{Admin},{Dealer}";
}
