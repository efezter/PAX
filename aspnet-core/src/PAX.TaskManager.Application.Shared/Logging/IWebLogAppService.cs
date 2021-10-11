using Abp.Application.Services;
using PAX.TaskManager.Dto;
using PAX.TaskManager.Logging.Dto;

namespace PAX.TaskManager.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
