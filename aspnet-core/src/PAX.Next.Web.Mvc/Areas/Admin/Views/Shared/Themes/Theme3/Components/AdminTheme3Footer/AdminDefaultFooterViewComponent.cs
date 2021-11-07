using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PAX.Next.Web.Areas.Admin.Models.Layout;
using PAX.Next.Web.Session;
using PAX.Next.Web.Views;

namespace PAX.Next.Web.Areas.Admin.Views.Shared.Themes.Theme3.Components.AdminTheme3Footer
{
    public class AdminTheme3FooterViewComponent : NextViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AdminTheme3FooterViewComponent(IPerRequestSessionCache sessionCache)
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
