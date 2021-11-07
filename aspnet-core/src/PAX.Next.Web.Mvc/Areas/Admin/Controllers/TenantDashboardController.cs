using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAX.Next.Authorization;
using PAX.Next.DashboardCustomization;
using System.Threading.Tasks;
using PAX.Next.Web.Areas.Admin.Startup;

namespace PAX.Next.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class TenantDashboardController : CustomizableDashboardControllerBase
    {
        public TenantDashboardController(DashboardViewConfiguration dashboardViewConfiguration, 
            IDashboardCustomizationAppService dashboardCustomizationAppService) 
            : base(dashboardViewConfiguration, dashboardCustomizationAppService)
        {

        }

        public async Task<ActionResult> Index()
        {
            return await GetView(NextDashboardCustomizationConsts.DashboardNames.DefaultTenantDashboard);
        }
    }
}