using System.Collections.Generic;
using Abp.Application.Services.Dto;
using PAX.TaskManager.Configuration.Host.Dto;
using PAX.TaskManager.Editions.Dto;

namespace PAX.TaskManager.Web.Areas.Admin.Models.HostSettings
{
    public class HostSettingsViewModel
    {
        public HostSettingsEditDto Settings { get; set; }

        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public List<ComboboxItemDto> TimezoneItems { get; set; }

        public List<string> EnabledSocialLoginSettings { get; set; } = new List<string>();
    }
}