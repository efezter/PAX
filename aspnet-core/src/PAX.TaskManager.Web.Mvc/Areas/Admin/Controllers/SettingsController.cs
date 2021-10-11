using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Runtime.Session;
using Abp.Timing;
using Microsoft.AspNetCore.Mvc;
using PAX.TaskManager.Authorization;
using PAX.TaskManager.Authorization.Users;
using PAX.TaskManager.Configuration;
using PAX.TaskManager.Configuration.Tenants;
using PAX.TaskManager.MultiTenancy;
using PAX.TaskManager.Timing;
using PAX.TaskManager.Timing.Dto;
using PAX.TaskManager.Web.Areas.Admin.Models.Settings;
using PAX.TaskManager.Web.Controllers;

namespace PAX.TaskManager.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Tenant_Settings)]
    public class SettingsController : TaskManagerControllerBase
    {
        private readonly UserManager _userManager;
        private readonly TenantManager _tenantManager;
        private readonly IAppConfigurationAccessor _configurationAccessor;
        private readonly ITenantSettingsAppService _tenantSettingsAppService;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly ITimingAppService _timingAppService;

        public SettingsController(
            ITenantSettingsAppService tenantSettingsAppService,
            IMultiTenancyConfig multiTenancyConfig,
            ITimingAppService timingAppService, 
            UserManager userManager, 
            TenantManager tenantManager,
            IAppConfigurationAccessor configurationAccessor)
        {
            _tenantSettingsAppService = tenantSettingsAppService;
            _multiTenancyConfig = multiTenancyConfig;
            _timingAppService = timingAppService;
            _userManager = userManager;
            _tenantManager = tenantManager;
            _configurationAccessor = configurationAccessor;
        }

        public async Task<ActionResult> Index()
        {
            var output = await _tenantSettingsAppService.GetAllSettings();
            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;

            var timezoneItems = await _timingAppService.GetTimezoneComboboxItems(new GetTimezoneComboboxItemsInput
            {
                DefaultTimezoneScope = SettingScopes.Tenant,
                SelectedTimezoneId = await SettingManager.GetSettingValueForTenantAsync(TimingSettingNames.TimeZone, AbpSession.GetTenantId())
            });

            var user = await _userManager.GetUserAsync(AbpSession.ToUserIdentifier());

            ViewBag.CurrentUserEmail = user.EmailAddress;

            var tenant = await _tenantManager.FindByIdAsync(AbpSession.GetTenantId());
            ViewBag.TenantId = tenant.Id;
            ViewBag.TenantLogoId = tenant.LogoId;
            ViewBag.TenantCustomCssId = tenant.CustomCssId;

            var model = new SettingsViewModel
            {
                Settings = output,
                TimezoneItems = timezoneItems
            };

            AddEnabledSocialLogins(model);
            
            return View(model);
        }
        
        private void AddEnabledSocialLogins(SettingsViewModel model)
        {
            if (!bool.Parse(_configurationAccessor.Configuration["Authentication:AllowSocialLoginSettingsPerTenant"]))
            {
                return;
            }

            if (bool.Parse(_configurationAccessor.Configuration["Authentication:Facebook:IsEnabled"]))
            {
                model.EnabledSocialLoginSettings.Add("Facebook");
            }

            if (bool.Parse(_configurationAccessor.Configuration["Authentication:Google:IsEnabled"]))
            {
                model.EnabledSocialLoginSettings.Add("Google");
            }

            if (bool.Parse(_configurationAccessor.Configuration["Authentication:Twitter:IsEnabled"]))
            {
                model.EnabledSocialLoginSettings.Add("Twitter");
            }

            if (bool.Parse(_configurationAccessor.Configuration["Authentication:Microsoft:IsEnabled"]))
            {
                model.EnabledSocialLoginSettings.Add("Microsoft");
            }
            
            if (bool.Parse(_configurationAccessor.Configuration["Authentication:OpenId:IsEnabled"]))
            {
                model.EnabledSocialLoginSettings.Add("OpenId");
            }
            
            if (bool.Parse(_configurationAccessor.Configuration["Authentication:WsFederation:IsEnabled"]))
            {
                model.EnabledSocialLoginSettings.Add("WsFederation");
            }
        }
    }
}