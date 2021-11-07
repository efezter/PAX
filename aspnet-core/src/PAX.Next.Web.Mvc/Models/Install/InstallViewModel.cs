using System.Collections.Generic;
using Abp.Localization;
using PAX.Next.Install.Dto;

namespace PAX.Next.Web.Models.Install
{
    public class InstallViewModel
    {
        public List<ApplicationLanguage> Languages { get; set; }

        public AppSettingsJsonDto AppSettingsJson { get; set; }
    }
}
