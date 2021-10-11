using System.Collections.Generic;
using Abp.Application.Services.Dto;
using PAX.TaskManager.Configuration.Tenants.Dto;

namespace PAX.TaskManager.Web.Areas.Admin.Models.Settings
{
    public class SettingsViewModel
    {
        public TenantSettingsEditDto Settings { get; set; }
        
        public List<ComboboxItemDto> TimezoneItems { get; set; }
        
        public List<string> EnabledSocialLoginSettings { get; set; } = new List<string>();
    }
}