using CarShop.Common;
using CarShop.Data;
using CarShop.Models.Accounts;
using CarShop.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Controllers;

[Authorize]
public class ShoppingController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public ShoppingController
        (
            ApplicationDbContext context,
            UserManager<AppUser> userManager
        )
    {
        _userManager = userManager;
        _context = context;
    }

    // GET: /Shopping
    public async Task<IActionResult> Index()
    {
        var carModels = await _context.CarModels.AsNoTracking().Include(cm => cm.CarBrand).Include(cm => cm.AppUser).ToArrayAsync();
        return View(carModels);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToWishList(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        var product = await _context.CarModels.FirstOrDefaultAsync(c => c.Id == id);

        var cartItem = new WishCart
        {
            AppUser = user,
            CarModel = product,
        };

        await _context.WishCarts.AddAsync(cartItem);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        return RedirectPermanent(Request.Headers.Referer.ToString());
    }

    public async Task<IActionResult> RemoveFromWishList(int id)
    {
        var wishCart = await _context.WishCarts.FirstOrDefaultAsync(cart => cart.Id == id);
        _context.WishCarts.Remove(wishCart);


        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        return RedirectPermanent(Request.Headers.Referer.ToString());
    }


    // GET: /Shopping/Dealers
    public async Task<IActionResult> Dealers()
    {
        var dealers = (await _userManager.GetUsersInRoleAsync(CustomRoles.Dealer)).Where(d => d.IsFinishedRegistration);
        foreach (var dealer in dealers)
        {
            _context.Entry(dealer).Collection(d => d.CarModels).Load();
            foreach (var cm in dealer.CarModels)
            {
                _context.Entry(cm).Reference(cm => cm.CarBrand).Load();
            }
        }
        return View(dealers.ToArray());
    }

    //GET: /Shopping/Dealers/{id}
    public async Task<IActionResult> Dealer(string? id)
    {
        if (id == null) return NotFound();
        var dealer = await _userManager.Users.Include(u => u.CarModels).ThenInclude(cm => cm.CarBrand).FirstOrDefaultAsync(d => d.Id == id);
        if (dealer == null) return NotFound();
        return View("Dealer", dealer);
    }
}
