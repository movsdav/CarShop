using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarShop.Areas.Admin.Views.Shared;

public static class ManageNavPages
{
    public static string Users => "Users";
    public static string CarBrands => "CarBrands";

    public static string UsersNavClass(ViewContext context) => PageNavClass(context, Users);
    public static string CarBrandsNavClass(ViewContext context) => PageNavClass(context, CarBrands);


    public static string PageNavClass(ViewContext context, string page)
    {
        var activePage = context.ViewData["ActivePage"] as string ?? Path.GetFileNameWithoutExtension(context.ActionDescriptor.DisplayName);
        return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
    }
}
