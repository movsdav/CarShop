using CarShop.Models.Accounts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarShop.Models.Product;

public class CarModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public string? ImgUrl { get; set; }
    public string? ImgFileName { get; set; }


    public string? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    [Display(Name="Brand")]
    public int CarBrandId { get; set; }
    [Display(Name = "Brand")]
    public CarBrand? CarBrand { get; set; }

    [NotMapped]
    [Display(Name = "Image")]
    public IFormFile? ImgFile { get; set; }
    public string GeneratePrefix() => $"Product_{Guid.NewGuid()}_";

    public CarModel()
    {
        
    }
}
