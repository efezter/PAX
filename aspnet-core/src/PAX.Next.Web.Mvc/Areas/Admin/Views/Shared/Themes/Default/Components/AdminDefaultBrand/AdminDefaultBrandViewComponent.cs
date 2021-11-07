using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PAX.Next.Web.Areas.Admin.Models.Layout;
using PAX.Next.Web.Session;
using PAX.Next.Web.Views;

namespace PAX.Next.Web.Areas.Admin.Views.Shared.Themes.Default.Components.AdminDefaultBrand
{
    public class AdminDefaultBrandViewComponent : NextViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AdminDefaultBrandViewComponent(IPerRequestSessionCache sessionCache)
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
