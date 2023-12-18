using CarShop.Areas.Admin.ViewModel;
using CarShop.Cloud;
using CarShop.Data;
using CarShop.Models.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Areas.Admin.Controllers;

[Area("Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly FileService _fileService;

    public AdminController(UserManager<AppUser> userManager, ApplicationDbContext context, FileService fileService)
    {
        _userManager = userManager;
        _context = context;
        _fileService = fileService;
    }

    [HttpPost]
    public async Task<IActionResult> UnblockUser(string id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);
        if (user != null)
        {
            await _userManager.SetLockoutEndDateAsync(user, null);
            return RedirectPermanent(Request.Headers.Referer.ToString());
        }
        else
        {
            return NotFound(user);
        }
    }

    [HttpPost]
    public async Task<IActionResult> BlockUser(string id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);
        if (user != null)
        {
            var isPermanent = Request.Form.TryGetValue("blockDays", out var value);
            var days = Convert.ToDouble(value);

            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.AddDays(days));

            return RedirectPermanent(Request.Headers.Referer.ToString());
        }
        else
        {
            return NotFound(user);
        }
    }

    public async Task<IActionResult> RemoveUser(string id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles != null)
                await _userManager.RemoveFromRolesAsync(user, userRoles);

            if (user.ImgFileName != null)
            {
                await _fileService.DeleteAsync(user.ImgFileName);
            }

            await _userManager.DeleteAsync(user);

            return RedirectPermanent(Request.Headers.Referer.ToString());
        }
        else
        {
            return NotFound();
        }
    }

    public async Task<IActionResult> Index()
    {
        var myUser = await _userManager.GetUserAsync(User);
        var users = await _userManager.Users.AsNoTracking().Where(user => user.Id != myUser.Id).ToArrayAsync();
        var usersViewModel = new List<UserRolesViewModel>();

        foreach (var user in users)
        {
            usersViewModel.Add(new UserRolesViewModel() { User = user, Roles = await _userManager.GetRolesAsync(user) });
        }

        return View(usersViewModel);
    }

    public async Task<IActionResult> Details(string id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);
        var roles = await _userManager.GetRolesAsync(user);

        return View(new UserRolesViewModel() { User = user, Roles = roles });
    }

    public async Task<IActionResult> CarBrands()
    {
        var carBrands = await _context.CarBrands.ToArrayAsync();
        return View(carBrands);
    }
}
