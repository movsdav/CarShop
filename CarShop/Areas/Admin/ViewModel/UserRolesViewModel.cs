using CarShop.Models.Accounts;

namespace CarShop.Areas.Admin.ViewModel;

public class UserRolesViewModel
{
    public AppUser User { get; set; }
    public IEnumerable<string> Roles { get; set; }
}
