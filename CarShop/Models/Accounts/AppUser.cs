using CarShop.Data;
using CarShop.Models.Product;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarShop.Models.Accounts;

public class AppUser : IdentityUser
{
    [Display(Name ="First Name")]
    public string? FirstName { get; set; } = default;
    [Display(Name = "Last Name")]
    public string? LastName { get; set; } = default;
    public decimal Balance { get; set; }
    public string? ImgUrl { get; set; }
    public string? ImgFileName { get; set; }
    public bool IsFinishedRegistration { get; set; } = false;

    public List<CarModel>? CarModels { get; set; } = null;
    public List<WishCart> WishCartItems { get; set; }


    [NotMapped]
    public IFormFile ImgFile { get; set; }
    public string GeneratePrefix() => $"ProfileAvatar_{Id}_";

    public AppUser()
    {
        
    }
}
