using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using PAX.Next.TaskManager.Exporting;
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
    [AbpAuthorize(AppPermissions.Pages_TaskStatuses)]
    public class TaskStatusesAppService : NextAppServiceBase, ITaskStatusesAppService
    {
        private readonly IRepository<TaskStatus> _taskStatusRepository;
        private readonly ITaskStatusesExcelExporter _taskStatusesExcelExporter;

        public TaskStatusesAppService(IRepository<TaskStatus> taskStatusRepository, ITaskStatusesExcelExporter taskStatusesExcelExporter)
        {
            _taskStatusRepository = taskStatusRepository;
            _taskStatusesExcelExporter = taskStatusesExcelExporter;

        }

        public async Task<PagedResultDto<GetTaskStatusForViewDto>> GetAll(GetAllTaskStatusesInput input)
        {

            var filteredTaskStatuses = _taskStatusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var pagedAndFilteredTaskStatuses = filteredTaskStatuses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var taskStatuses = from o in pagedAndFilteredTaskStatuses
                               select new
                               {

                                   o.Name,
                                   o.IconUrl,
                                   Id = o.Id
                               };

            var totalCount = await filteredTaskStatuses.CountAsync();

            var dbList = await taskStatuses.ToListAsync();
            var results = new List<GetTaskStatusForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTaskStatusForViewDto()
                {
                    TaskStatus = new TaskStatusDto
                    {

                        Name = o.Name,
                        IconUrl = o.IconUrl,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTaskStatusForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetTaskStatusForViewDto> GetTaskStatusForView(int id)
        {
            var taskStatus = await _taskStatusRepository.GetAsync(id);

            var output = new GetTaskStatusForViewDto { TaskStatus = ObjectMapper.Map<TaskStatusDto>(taskStatus) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TaskStatuses_Edit)]
        public async Task<GetTaskStatusForEditOutput> GetTaskStatusForEdit(EntityDto input)
        {
            var taskStatus = await _taskStatusRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTaskStatusForEditOutput { TaskStatus = ObjectMapper.Map<CreateOrEditTaskStatusDto>(taskStatus) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTaskStatusDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TaskStatuses_Create)]
        protected virtual async Task Create(CreateOrEditTaskStatusDto input)
        {
            var taskStatus = ObjectMapper.Map<TaskStatus>(input);

            await _taskStatusRepository.InsertAsync(taskStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskStatuses_Edit)]
        protected virtual async Task Update(CreateOrEditTaskStatusDto input)
        {
            var taskStatus = await _taskStatusRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, taskStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskStatuses_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _taskStatusRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTaskStatusesToExcel(GetAllTaskStatusesForExcelInput input)
        {

            var filteredTaskStatuses = _taskStatusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var query = (from o in filteredTaskStatuses
                         select new GetTaskStatusForViewDto()
                         {
                             TaskStatus = new TaskStatusDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var taskStatusListDtos = await query.ToListAsync();

            return _taskStatusesExcelExporter.ExportToFile(taskStatusListDtos);
        }

    }
}