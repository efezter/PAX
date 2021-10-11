using System.Collections.Generic;
using Abp.Localization;
using PAX.TaskManager.Install.Dto;

namespace PAX.TaskManager.Web.Models.Install
{
    public class InstallViewModel
    {
        public List<ApplicationLanguage> Languages { get; set; }

        public AppSettingsJsonDto AppSettingsJson { get; set; }
    }
}
