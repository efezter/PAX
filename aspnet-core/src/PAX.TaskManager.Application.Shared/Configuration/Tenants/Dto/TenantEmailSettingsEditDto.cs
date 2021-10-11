using Abp.Auditing;
using PAX.TaskManager.Configuration.Dto;

namespace PAX.TaskManager.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}