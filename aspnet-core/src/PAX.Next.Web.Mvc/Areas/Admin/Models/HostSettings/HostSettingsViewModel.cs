using System.Collections.Generic;
using Abp.Application.Services.Dto;
using PAX.Next.Configuration.Host.Dto;
using PAX.Next.Editions.Dto;

namespace PAX.Next.Web.Areas.Admin.Models.HostSettings
{
    public class HostSettingsViewModel
    {
        public HostSettingsEditDto Settings { get; set; }

        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public List<ComboboxItemDto> TimezoneItems { get; set; }

        public List<string> EnabledSocialLoginSettings { get; set; } = new List<string>();
    }
}