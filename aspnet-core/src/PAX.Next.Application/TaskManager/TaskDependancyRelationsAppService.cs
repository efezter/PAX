using PAX.Next.TaskManager;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using PAX.Next.TaskManager.Dtos;
using PAX.Next.Dto;
using Abp.Application.Services.Dto;
using PAX.Next.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using PAX.Next.Storage;

namespace PAX.Next.TaskManager
{
    [AbpAuthorize(AppPermissions.Pages_TaskDependancyRelations)]
    public class TaskDependancyRelationsAppService : NextAppServiceBase, ITaskDependancyRelationsAppService
    {
        private readonly IRepository<TaskDependancyRelation> _taskDependancyRelationRepository;
        private readonly IRepository<PaxTask, int> _lookup_paxTaskRepository;

        public TaskDependancyRelationsAppService(IRepository<TaskDependancyRelation> taskDependancyRelationRepository, IRepository<PaxTask, int> lookup_paxTaskRepository)
        {
            _taskDependancyRelationRepository = taskDependancyRelationRepository;
            _lookup_paxTaskRepository = lookup_paxTaskRepository;

        }

        public async Task<PagedResultDto<GetTaskDependancyRelationForViewDto>> GetAll(GetAllTaskDependancyRelationsInput input)
        {

            var filteredTaskDependancyRelations = _taskDependancyRelationRepository.GetAll()
                        .Include(e => e.PaxTaskFk)
                        .Include(e => e.DependOnTaskFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.TaskIdFilter != 0, e => e.PaxTaskFk != null && e.PaxTaskFk.Id== input.TaskIdFilter);

            var pagedAndFilteredTaskDependancyRelations = filteredTaskDependancyRelations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var taskDependancyRelations = from o in pagedAndFilteredTaskDependancyRelations
                                          join o1 in _lookup_paxTaskRepository.GetAll() on o.PaxTaskId equals o1.Id into j1
                                          from s1 in j1.DefaultIfEmpty()

                                          join o2 in _lookup_paxTaskRepository.GetAll() on o.DependOnTaskId equals o2.Id into j2
                                          from s2 in j2.DefaultIfEmpty()

                                          select new
                                          {

                                              Id = o.Id,
                                              PaxTaskHeader = s1 == null || s1.Header == null ? "" : s1.Header.ToString(),
                                              PaxTaskHeader2 = s2 == null || s2.Header == null ? "" : s2.Header.ToString()
                                          };

            var totalCount = await filteredTaskDependancyRelations.CountAsync();

            var dbList = await taskDependancyRelations.ToListAsync();
            var results = new List<GetTaskDependancyRelationForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTaskDependancyRelationForViewDto()
                {
                    TaskDependancyRelation = new TaskDependancyRelationDto
                    {

                        Id = o.Id,
                    },
                    PaxTaskHeader = o.PaxTaskHeader,
                    PaxTaskHeader2 = o.PaxTaskHeader2
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTaskDependancyRelationForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<List<TaskDependancyRelationDto>> GetTasksDependecies(int taskId)
        {
            var filteredTaskDependancyRelations = _taskDependancyRelationRepository.GetAll()
                      .WhereIf(taskId != 0, e => false || e.PaxTaskId == taskId);

            return (from o in filteredTaskDependancyRelations
                    select new TaskDependancyRelationDto
                    {
                        DependOnHeader = o.DependOnTaskFk.Header,
                        DependOnTaskId = o.DependOnTaskId,
                        Id = o.Id

                    }).ToList();
        }

        public async Task<GetTaskDependancyRelationForViewDto> GetTaskDependancyRelationForView(int id)
        {
            var taskDependancyRelation = await _taskDependancyRelationRepository.GetAsync(id);

            var output = new GetTaskDependancyRelationForViewDto { TaskDependancyRelation = ObjectMapper.Map<TaskDependancyRelationDto>(taskDependancyRelation) };

            if (output.TaskDependancyRelation.PaxTaskId != null)
            {
                var _lookupPaxTask = await _lookup_paxTaskRepository.FirstOrDefaultAsync((int)output.TaskDependancyRelation.PaxTaskId);
                output.PaxTaskHeader = _lookupPaxTask?.Header?.ToString();
            }

            if (output.TaskDependancyRelation.DependOnTaskId != null)
            {
                var _lookupPaxTask = await _lookup_paxTaskRepository.FirstOrDefaultAsync((int)output.TaskDependancyRelation.DependOnTaskId);
                output.PaxTaskHeader2 = _lookupPaxTask?.Header?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TaskDependancyRelations_Edit)]
        public async Task<GetTaskDependancyRelationForEditOutput> GetTaskDependancyRelationForEdit(EntityDto input)
        {
            var taskDependancyRelation = await _taskDependancyRelationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTaskDependancyRelationForEditOutput { TaskDependancyRelation = ObjectMapper.Map<CreateOrEditTaskDependancyRelationDto>(taskDependancyRelation) };

            if (output.TaskDependancyRelation.PaxTaskId != null)
            {
                var _lookupPaxTask = await _lookup_paxTaskRepository.FirstOrDefaultAsync((int)output.TaskDependancyRelation.PaxTaskId);
                output.PaxTaskHeader = _lookupPaxTask?.Header?.ToString();
            }

            if (output.TaskDependancyRelation.DependOnTaskId != null)
            {
                var _lookupPaxTask = await _lookup_paxTaskRepository.FirstOrDefaultAsync((int)output.TaskDependancyRelation.DependOnTaskId);
                output.PaxTaskHeader2 = _lookupPaxTask?.Header?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTaskDependancyRelationDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TaskDependancyRelations_Create)]
        protected virtual async Task Create(CreateOrEditTaskDependancyRelationDto input)
        {
            var taskDependancyRelation = ObjectMapper.Map<TaskDependancyRelation>(input);

            await _taskDependancyRelationRepository.InsertAsync(taskDependancyRelation);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskDependancyRelations_Edit)]
        protected virtual async Task Update(CreateOrEditTaskDependancyRelationDto input)
        {
            var taskDependancyRelation = await _taskDependancyRelationRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, taskDependancyRelation);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskDependancyRelations_Delete)]
        public async Task Delete(int id)
        {
            await _taskDependancyRelationRepository.DeleteAsync(id);
        }
        [AbpAuthorize(AppPermissions.Pages_TaskDependancyRelations)]
        public async Task<List<TaskDependancyRelationPaxTaskLookupTableDto>> GetAllPaxTaskForTableDropdown()
        {
            return await _lookup_paxTaskRepository.GetAll()
                .Select(paxTask => new TaskDependancyRelationPaxTaskLookupTableDto
                {
                    Id = paxTask.Id,
                    DisplayName = paxTask == null || paxTask.Header == null ? "" : paxTask.Header.ToString()
                }).ToListAsync();
        }

    }
}