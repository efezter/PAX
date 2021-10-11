using System.Threading.Tasks;
using Abp.Application.Services;
using PAX.TaskManager.Configuration.Host.Dto;

namespace PAX.TaskManager.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
