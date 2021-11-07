using System.Threading.Tasks;
using Abp.Application.Services;
using PAX.Next.Install.Dto;

namespace PAX.Next.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}