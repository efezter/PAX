using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAX.TaskManager.Authorization;
using PAX.TaskManager.DashboardCustomization;
using System.Threading.Tasks;
using PAX.TaskManager.Web.Areas.Admin.Startup;

namespace PAX.TaskManager.Web.Areas.Admin.Controllers
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
            return await GetView(TaskManagerDashboardCustomizationConsts.DashboardNames.DefaultTenantDashboard);
        }
    }
}