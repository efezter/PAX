using System.Threading.Tasks;
using Abp.Application.Services;
using PAX.TaskManager.Configuration.Tenants.Dto;

namespace PAX.TaskManager.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
