using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarShop.Models.Product;

public class CarBrand
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public string? ImgUrl { get; set; }
    public string? ImgFileName { get; set; }

    [NotMapped]
    [Display(Name="Image")]
    public IFormFile? ImgFile { get; set; }
    

    public List<CarModel>? CarModels { get; set; }

    public string GeneratePrefix() => $"Logo_{Guid.NewGuid()}_";

    public CarBrand()
    {
        
    }
}
