using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using PAX.Next.Authorization;
using PAX.Next.Authorization.Users;
using PAX.Next.Notifications;
using PAX.Next.TaskManager.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace PAX.Next.TaskManager
{
    [AbpAuthorize(AppPermissions.Pages_TaskHistories)]
    public class TaskHistoriesAppService : NextAppServiceBase, ITaskHistoriesAppService
    {
        private readonly IRepository<TaskHistory> _taskHistoryRepository;
        private readonly IRepository<PaxTask, int> _lookup_paxTaskRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        

        public TaskHistoriesAppService(IRepository<TaskHistory> taskHistoryRepository, IRepository<PaxTask, int> lookup_paxTaskRepository, IRepository<User, long> lookup_userRepository, IAppNotifier appNotifier)
        {
            _taskHistoryRepository = taskHistoryRepository;
            _lookup_paxTaskRepository = lookup_paxTaskRepository;
            _lookup_userRepository = lookup_userRepository;
           

        }

        public async Task<PagedResultDto<GetTaskHistoryForViewDto>> GetAll(GetAllTaskHistoriesInput input)
        {

            var filteredTaskHistories = _taskHistoryRepository.GetAll()
                        .Include(e => e.PaxTaskFk)
                        .Include(e => e.CreatedUserFk)
                        .WhereIf(input.PaxTaskIdFilter != 0, e => e.PaxTaskFk != null && e.PaxTaskFk.Id == input.PaxTaskIdFilter)
                        .WhereIf(input.UserIdFilter != 0, e => e.CreatedUserFk != null && e.CreatedUserFk.Id == input.UserIdFilter);

            var pagedAndFilteredTaskHistories = filteredTaskHistories
                .OrderBy(input.Sorting ?? "id asc");

            var taskHistories = from o in pagedAndFilteredTaskHistories
                                join o1 in _lookup_paxTaskRepository.GetAll() on o.PaxTaskId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _lookup_userRepository.GetAll() on o.CreatedUser equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                select new
                                {

                                    Id = o.Id,
                                    ChangeType = o.ChangeType,
                                    CreatedDate = o.CreatedDate,
                                    NewValue = o.NewValue,
                                    FieldName = o.FieldName,
                                    OldValue = o.NewValue,
                                    UserName = s2.FullName
                                };

            var dbList = await taskHistories.ToListAsync();
            var results = new List<GetTaskHistoryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTaskHistoryForViewDto()
                {
                    TaskHistory = new TaskHistoryDto
                    {
                        Id = o.Id,
                        ChangeType = o.ChangeType,
                        CreatedTime = o.CreatedDate,
                        NewValue = o.NewValue,
                        FieldName = o.FieldName,
                        OldValue = o.OldValue
                    },
                    UserName = o.UserName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTaskHistoryForViewDto>(
                dbList.Count,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_TaskHistories_Edit)]
        public async Task<GetTaskHistoryForEditOutput> GetTaskHistoryForEdit(EntityDto input)
        {
            var taskHistory = await _taskHistoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTaskHistoryForEditOutput { TaskHistory = ObjectMapper.Map<CreateOrEditTaskHistoryDto>(taskHistory) };

            if (output.TaskHistory.PaxTaskId != null)
            {
                var _lookupPaxTask = await _lookup_paxTaskRepository.FirstOrDefaultAsync((int)output.TaskHistory.PaxTaskId);
                output.PaxTaskHeader = _lookupPaxTask?.Header?.ToString();
            }

            if (output.TaskHistory.CreatedUser != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.TaskHistory.CreatedUser);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTaskHistoryDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_TaskHistories_Create)]
        protected virtual async Task Create(CreateOrEditTaskHistoryDto input)
        {
            var taskHistory = ObjectMapper.Map<TaskHistory>(input);

            await _taskHistoryRepository.InsertAsync(taskHistory);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskHistories_Edit)]
        protected virtual async Task Update(CreateOrEditTaskHistoryDto input)
        {
            var taskHistory = await _taskHistoryRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, taskHistory);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskHistories_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _taskHistoryRepository.DeleteAsync(input.Id);
        }
        [AbpAuthorize(AppPermissions.Pages_TaskHistories)]
        public async Task<List<TaskHistoryPaxTaskLookupTableDto>> GetAllPaxTaskForTableDropdown()
        {
            return await _lookup_paxTaskRepository.GetAll()
                .Select(paxTask => new TaskHistoryPaxTaskLookupTableDto
                {
                    Id = paxTask.Id,
                    DisplayName = paxTask == null || paxTask.Header == null ? "" : paxTask.Header.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_TaskHistories)]
        public async Task<List<TaskHistoryUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _lookup_userRepository.GetAll()
                .Select(user => new TaskHistoryUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null ? "" : user.Name.ToString()
                }).ToListAsync();
        }

    }
}