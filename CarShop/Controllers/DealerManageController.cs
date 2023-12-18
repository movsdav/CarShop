using CarShop.Data;
using CarShop.Models.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Controllers;

public class DealerManageController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public DealerManageController
        (
            ApplicationDbContext context,
            UserManager<AppUser> userManager
        )
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        await _context.Entry(user).Collection(user => user.CarModels).LoadAsync();
        var products = user.CarModels;

        ViewData["CarBrands"] = new SelectList(_context.CarBrands,"Id","Name");
        
        foreach (var el in products)
        {
            await _context.Entry(el).Reference(e => e.CarBrand).LoadAsync();
            
        }

        return View(products);
    }
}
