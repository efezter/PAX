using Abp.Application.Services;
using PAX.Next.TaskManager.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAX.Next.TaskManager
{
    public interface IReportAppService : IApplicationService
    {
        Task<List<ReportTopStat>> GetPersonalSummaryWidget(long userId);

    }
}