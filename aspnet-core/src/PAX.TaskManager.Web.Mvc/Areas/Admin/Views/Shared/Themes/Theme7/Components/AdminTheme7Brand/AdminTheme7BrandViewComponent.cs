using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PAX.TaskManager.Web.Areas.Admin.Models.Layout;
using PAX.TaskManager.Web.Session;
using PAX.TaskManager.Web.Views;

namespace PAX.TaskManager.Web.Areas.Admin.Views.Shared.Themes.Theme7.Components.AdminTheme7Brand
{
    public class AdminTheme7BrandViewComponent : TaskManagerViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AdminTheme7BrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(headerModel);
        }
    }
}
