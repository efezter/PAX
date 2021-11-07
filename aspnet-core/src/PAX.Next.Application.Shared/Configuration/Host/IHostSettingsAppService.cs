using System.Threading.Tasks;
using Abp.Application.Services;
using PAX.Next.Configuration.Host.Dto;

namespace PAX.Next.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
