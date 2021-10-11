using System.Threading.Tasks;
using Abp.Application.Services;
using PAX.TaskManager.Install.Dto;

namespace PAX.TaskManager.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}