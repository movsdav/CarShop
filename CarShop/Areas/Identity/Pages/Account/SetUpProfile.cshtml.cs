using CarShop.Cloud;
using CarShop.Data;
using CarShop.Models.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CarShop.Areas.Identity.Pages.Account;

public class SetUpProfileModel : PageModel
{
    private readonly UserManager<AppUser> _userManager;
    private readonly FileService _fileService;
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<AppUser> _signInManager;

    public SetUpProfileModel
        (
            UserManager<AppUser> userManager,
            FileService fileService,
            ApplicationDbContext context,
            SignInManager<AppUser> signInManager
        )
    {
        _userManager = userManager;
        _fileService = fileService;
        _context = context;
        _signInManager = signInManager;
    }

    [ModelBinder]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Profile Avatar")]
        public IFormFile? ImgFile { get; set; }
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var userId = Request.Query["userId"].ToString();
        var _user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        // if (_user == null) ;

        if (ModelState.IsValid)
        {
            _user.FirstName = Input.FirstName;
            _user.LastName = Input.LastName;

            if(Input.ImgFile != null)
            {
                var uploudResult =
                    await _fileService.UploadAsync(Input.ImgFile,_user.GeneratePrefix());

                _user.ImgFileName = uploudResult.Blob.Name;
                _user.ImgUrl = uploudResult.Blob.Uri;
            }

            _user.IsFinishedRegistration = true;
            await _userManager.UpdateAsync(_user);
            await _signInManager.SignInAsync(_user, isPersistent: false);
            return RedirectToActionPermanent("Index","Home");
        }

        return Page();
    }
}
