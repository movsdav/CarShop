using CarShop.Data;
using CarShop.Models.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.ViewComponents;

public class WishListViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public WishListViewComponent(UserManager<AppUser> userManager, ApplicationDbContext context)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await _userManager.GetUserAsync(Request.HttpContext.User);
        await _context.Entry(user).Collection(user => user.WishCartItems).LoadAsync();
        foreach (var model in user.WishCartItems)
        {
            await _context.Entry(model).Reference(m => m.CarModel).LoadAsync();
            await _context.Entry(model.CarModel).Reference(cm => cm.CarBrand).LoadAsync();
        }

        return View("Default",user.WishCartItems);
    }
}
