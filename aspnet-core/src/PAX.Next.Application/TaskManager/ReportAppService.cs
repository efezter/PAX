using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Organizations;
using Microsoft.EntityFrameworkCore;
using PAX.Next.Organizations.Dto;
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
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<OrganizationUnitRole, long> _organizationUnitRoleRepository;

        public ReportAppService(
            IRepository<PaxTask> paxTaskRepository, 
            IRepository<TaskStatus> taskStatusRepository,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository)
        {
            _paxTaskRepository = paxTaskRepository;
            _taskStatusRepository = taskStatusRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitRoleRepository = organizationUnitRoleRepository;
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

        public async Task<ListResultDto<OrganizationUnitDto>> GetOrganizationSchema()
        {
            var organizationUnits = await _organizationUnitRepository.GetAllListAsync();

            var organizationUnitMemberCounts = await _userOrganizationUnitRepository.GetAll()
                .GroupBy(x => x.OrganizationUnitId)
                .Select(groupedUsers => new
                {
                    organizationUnitId = groupedUsers.Key,
                    count = groupedUsers.Count()
                }).ToDictionaryAsync(x => x.organizationUnitId, y => y.count);


            var organizationUnitRoleCounts = await _organizationUnitRoleRepository.GetAll()
                .GroupBy(x => x.OrganizationUnitId)
                .Select(groupedRoles => new
                {
                    organizationUnitId = groupedRoles.Key,
                    count = groupedRoles.Count()
                }).ToDictionaryAsync(x => x.organizationUnitId, y => y.count);

            return new ListResultDto<OrganizationUnitDto>(
                organizationUnits.Select(ou =>
                {
                    var organizationUnitDto = ObjectMapper.Map<OrganizationUnitDto>(ou);
                    organizationUnitDto.MemberCount = organizationUnitMemberCounts.ContainsKey(ou.Id) ? organizationUnitMemberCounts[ou.Id] : 0;
                    organizationUnitDto.RoleCount = organizationUnitRoleCounts.ContainsKey(ou.Id) ? organizationUnitRoleCounts[ou.Id] : 0;
                    return organizationUnitDto;
                }).ToList());
        }
    }
}