using CarShop.Data;
using CarShop.Models;
using CarShop.Models.Accounts;
using CarShop.Models.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CarShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await _context.CarBrands.Take(5).ToArrayAsync();
            return View(cars);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RefillBalance()
        {
            var amount = Convert.ToDecimal(Request.Form["amount"]);
            var currentUser = await _userManager.GetUserAsync(User);
            currentUser.Balance += amount;
            await _userManager.UpdateAsync(currentUser);
            return RedirectPermanent(Request.Headers.Referer.ToString());
        }
    }
}
