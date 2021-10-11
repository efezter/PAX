using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PAX.TaskManager.Web.Areas.Admin.Models.Layout;
using PAX.TaskManager.Web.Views;

namespace PAX.TaskManager.Web.Areas.Admin.Views.Shared.Components.AdminChatToggler
{
    public class AdminChatTogglerViewComponent : TaskManagerViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string cssClass)
        {
            return Task.FromResult<IViewComponentResult>(View(new ChatTogglerViewModel
            {
                CssClass = cssClass
            }));
        }
    }
}
