using CarShop.Data;
using CarShop.Models.Accounts;
using CarShop.Models.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Controllers;

public class CheckoutController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _context;

    public CheckoutController(ApplicationDbContext context, UserManager<AppUser> userManager)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var user = await LoadUser();
        var wishCarts = user.WishCartItems;
        foreach (var cart in wishCarts)
        {
            await LoadCarModel(cart);
            await LoadCarBrand(cart.CarModel);
        }

        return View(wishCarts);
    }

    public async Task<IActionResult> Checkout(decimal totalPrice)
    {
        var user = await LoadUser();
        if (totalPrice <= user.Balance)
        {
            var wishCarts = user.WishCartItems;
            foreach (var cart in wishCarts)
            {
                await LoadCarModel(cart);
                await _context.Entry(cart.CarModel).Reference(cm => cm.AppUser).LoadAsync();

                cart.CarModel.AppUser.Balance += cart.CarModel.Price;
                user.Balance -= cart.CarModel.Price;

                _context.CarModels.Remove(cart.CarModel);
                _context.WishCarts.Remove(cart);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return RedirectToActionPermanent("Index", "Home");
        }
        else
        {
            throw new Exception("Not enough money");
        }

    }

    private async Task LoadCarBrand(CarModel carModel)
    {
        await _context.Entry(carModel).Reference(cm => cm.CarBrand).LoadAsync();
    }

    private async Task LoadCarModel(WishCart cart)
    {
        await _context.Entry(cart).Reference(c => c.CarModel).LoadAsync();
    }

    private async Task<AppUser> LoadUser()
    {
        var user = await _userManager.GetUserAsync(User);
        await _context.Entry(user).Collection(user => user.WishCartItems).LoadAsync();
        return user;
    }
}
