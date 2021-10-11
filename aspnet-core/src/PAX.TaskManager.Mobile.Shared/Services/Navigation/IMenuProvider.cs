using System.Collections.Generic;
using MvvmHelpers;
using PAX.TaskManager.Models.NavigationMenu;

namespace PAX.TaskManager.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}