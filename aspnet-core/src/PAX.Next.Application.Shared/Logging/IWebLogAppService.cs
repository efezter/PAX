using Abp.Application.Services;
using PAX.Next.Dto;
using PAX.Next.Logging.Dto;

namespace PAX.Next.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
