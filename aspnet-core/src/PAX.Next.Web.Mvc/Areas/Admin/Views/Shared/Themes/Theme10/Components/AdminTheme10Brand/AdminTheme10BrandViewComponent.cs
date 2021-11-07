using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PAX.Next.Web.Areas.Admin.Models.Layout;
using PAX.Next.Web.Session;
using PAX.Next.Web.Views;

namespace PAX.Next.Web.Areas.Admin.Views.Shared.Themes.Theme10.Components.AdminTheme10Brand
{
    public class AdminTheme10BrandViewComponent : NextViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AdminTheme10BrandViewComponent(IPerRequestSessionCache sessionCache)
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
