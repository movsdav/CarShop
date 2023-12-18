using CarShop.Cloud;
using CarShop.Data;
using CarShop.Models.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CarShop.Areas.Identity.Pages.Account.Manage;

public class ProfileDataModel : PageModel
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly FileService _fileService;

    public ProfileDataModel
        (
            UserManager<AppUser> userManager,
            SignInManager<AppUser> singInManager,
            FileService fileService
        )
    {
        _userManager = userManager;
        _signInManager = singInManager;
        _fileService = fileService;
    }


    [BindProperty]
    public string FirstName { get; set; }
    [BindProperty]
    public string LastName { get; set; }
    [BindProperty]
    public IFormFile? ImgFile { get; set; }
    public string? ImgUrl { get; set; }

    public async Task LoadAsync(AppUser user)
    {
        var _user = await _userManager.GetUserAsync(User);

        FirstName = _user.FirstName;
        LastName = _user.LastName;
        ImgUrl = _user.ImgUrl;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if(user == null)
        {
            return NotFound($"Unable load user with ID: {_userManager.GetUserId(User)}");
        }
        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound($"Unable load user with ID: {_userManager.GetUserId(User)}");

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        if(ImgFile != null)
        {
            var blob = await _fileService.GetBlobAsync(user.ImgFileName);

            if(blob != null)
            {
                await _fileService.DeleteAsync(blob.Name);
            }

            var uploadResult = await _fileService.UploadAsync(ImgFile,user.GeneratePrefix());

            user.ImgFileName = uploadResult.Blob.Name;
            user.ImgUrl = uploadResult.Blob.Uri;
        }

        user.FirstName = FirstName;
        user.LastName = LastName;
        await _userManager.UpdateAsync(user);
        await _signInManager.RefreshSignInAsync(user);
        return RedirectToPage();
    }
}
