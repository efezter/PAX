using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PAX.TaskManager.Web.Areas.Admin.Models.Layout;
using PAX.TaskManager.Web.Session;
using PAX.TaskManager.Web.Views;

namespace PAX.TaskManager.Web.Areas.Admin.Views.Shared.Themes.Theme2.Components.AdminTheme2Footer
{
    public class AdminTheme2FooterViewComponent : TaskManagerViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AdminTheme2FooterViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerModel = new FooterViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(footerModel);
        }
    }
}
