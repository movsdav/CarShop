using CarShop.Models.Accounts;

namespace CarShop.Models.Product;

public class WishCart
{
    public int Id { get; set; }

    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }

    public int? CarModelId { get; set; }
    public CarModel CarModel { get; set; }

    public WishCart()
    {
        
    }
}