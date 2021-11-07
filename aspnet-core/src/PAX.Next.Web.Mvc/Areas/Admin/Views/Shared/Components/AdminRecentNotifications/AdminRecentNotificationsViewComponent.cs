using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PAX.Next.Web.Areas.Admin.Models.Layout;
using PAX.Next.Web.Views;

namespace PAX.Next.Web.Areas.Admin.Views.Shared.Components.AdminRecentNotifications
{
    public class AdminRecentNotificationsViewComponent : NextViewComponent
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
