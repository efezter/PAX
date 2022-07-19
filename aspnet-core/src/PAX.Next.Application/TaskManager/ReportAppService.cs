using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using PAX.Next.Authorization;
using PAX.Next.TaskManager.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace PAX.Next.TaskManager
{
    //[AbpAuthorize(AppPermissions.Pages_Labels)]
    public class ReportAppService : NextAppServiceBase, IReportAppService
    {
        private readonly IRepository<PaxTask> _paxTaskRepository;
        private readonly IRepository<TaskStatus> _taskStatusRepository;

        public ReportAppService(IRepository<PaxTask> paxTaskRepository, IRepository<TaskStatus> taskStatusRepository)
        {
            _paxTaskRepository = paxTaskRepository;
            _taskStatusRepository = taskStatusRepository;
        }

        public async Task<List<ReportTopStat>> GetPersonalSummaryWidget(long userId)
        {
            var stats = from o in _taskStatusRepository.GetAll()
                         select new ReportTopStat
                         {
                             TaskStatusName = o.Name,
                             Count = (from t in _paxTaskRepository.GetAll() where t.AssigneeId == userId & t.TaskStatusId == o.Id select t.Id).Count(),
                         };

            return await stats.ToListAsync();
        }
    }
}