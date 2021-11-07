using System.Collections.Generic;
using Abp.Application.Services.Dto;
using PAX.Next.Configuration.Tenants.Dto;

namespace PAX.Next.Web.Areas.Admin.Models.Settings
{
    public class SettingsViewModel
    {
        public TenantSettingsEditDto Settings { get; set; }
        
        public List<ComboboxItemDto> TimezoneItems { get; set; }
        
        public List<string> EnabledSocialLoginSettings { get; set; } = new List<string>();
    }
}