using Abp.Auditing;
using PAX.Next.Configuration.Dto;

namespace PAX.Next.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}