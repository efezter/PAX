using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PAX.TaskManager.Web.Areas.Admin.Models.Layout;
using PAX.TaskManager.Web.Views;

namespace PAX.TaskManager.Web.Areas.Admin.Views.Shared.Components.AdminRecentNotifications
{
    public class AdminRecentNotificationsViewComponent : TaskManagerViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string cssClass)
        {
            var model = new RecentNotificationsViewModel
            {
                CssClass = cssClass
            };
            
            return Task.FromResult<IViewComponentResult>(View(model));
        }
    }
}
