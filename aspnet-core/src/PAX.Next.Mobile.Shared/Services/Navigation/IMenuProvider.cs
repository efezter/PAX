using System.Collections.Generic;
using MvvmHelpers;
using PAX.Next.Models.NavigationMenu;

namespace PAX.Next.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}