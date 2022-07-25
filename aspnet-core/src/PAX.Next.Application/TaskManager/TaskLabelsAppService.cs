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
    [AbpAuthorize(AppPermissions.Pages_TaskLabels)]
    public class TaskLabelsAppService : NextAppServiceBase, ITaskLabelsAppService
    {
        private readonly IRepository<TaskLabel> _taskLabelRepository;
        private readonly IRepository<PaxTask, int> _lookup_paxTaskRepository;
        private readonly IRepository<Label, int> _lookup_labelRepository;

        public TaskLabelsAppService(IRepository<TaskLabel> taskLabelRepository, IRepository<PaxTask, int> lookup_paxTaskRepository, IRepository<Label, int> lookup_labelRepository)
        {
            _taskLabelRepository = taskLabelRepository;
            _lookup_paxTaskRepository = lookup_paxTaskRepository;
            _lookup_labelRepository = lookup_labelRepository;

        }

        public async Task<PagedResultDto<GetTaskLabelForViewDto>> GetAll(GetAllTaskLabelsInput input)
        {

            var filteredTaskLabels = _taskLabelRepository.GetAll()
                        .Include(e => e.PaxTaskFk)
                        .Include(e => e.LabelFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaxTaskHeaderFilter), e => e.PaxTaskFk != null && e.PaxTaskFk.Header == input.PaxTaskHeaderFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LabelNameFilter), e => e.LabelFk != null && e.LabelFk.Name == input.LabelNameFilter);

            var pagedAndFilteredTaskLabels = filteredTaskLabels
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var taskLabels = from o in pagedAndFilteredTaskLabels
                             join o1 in _lookup_paxTaskRepository.GetAll() on o.PaxTaskId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             join o2 in _lookup_labelRepository.GetAll() on o.LabelId equals o2.Id into j2
                             from s2 in j2.DefaultIfEmpty()

                             select new
                             {

                                 Id = o.Id,
                                 PaxTaskHeader = s1 == null || s1.Header == null ? "" : s1.Header.ToString(),
                                 LabelName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                             };

            var totalCount = await filteredTaskLabels.CountAsync();

            var dbList = await taskLabels.ToListAsync();
            var results = new List<GetTaskLabelForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTaskLabelForViewDto()
                {
                    TaskLabel = new TaskLabelDto
                    {

                        Id = o.Id,
                    },
                    PaxTaskHeader = o.PaxTaskHeader,
                    LabelName = o.LabelName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTaskLabelForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_TaskLabels_Edit)]
        public async Task<GetTaskLabelForEditOutput> GetTaskLabelForEdit(EntityDto input)
        {
            var taskLabel = await _taskLabelRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTaskLabelForEditOutput { TaskLabel = ObjectMapper.Map<CreateOrEditTaskLabelDto>(taskLabel) };

            if (output.TaskLabel.PaxTaskId != 0)
            {
                var _lookupPaxTask = await _lookup_paxTaskRepository.FirstOrDefaultAsync((int)output.TaskLabel.PaxTaskId);
                output.PaxTaskHeader = _lookupPaxTask?.Header?.ToString();
            }

            if (output.TaskLabel.LabelId != 0)
            {
                var _lookupLabel = await _lookup_labelRepository.FirstOrDefaultAsync((int)output.TaskLabel.LabelId);
                output.LabelName = _lookupLabel?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTaskLabelDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TaskLabels_Create)]
        protected virtual async Task Create(CreateOrEditTaskLabelDto input)
        {
            var taskLabel = ObjectMapper.Map<TaskLabel>(input);

            await _taskLabelRepository.InsertAsync(taskLabel);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskLabels_Edit)]
        protected virtual async Task Update(CreateOrEditTaskLabelDto input)
        {
            var taskLabel = await _taskLabelRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, taskLabel);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskLabels_Delete)]
        public async Task Delete(int Id)
        {
            await _taskLabelRepository.DeleteAsync(Id);
        }

        [AbpAuthorize(AppPermissions.Pages_TaskLabels)]
        public async Task<List<TaskLabelLabelLookupTableDto>> GetAllLabelForTableDropdown()
        {
            return await _lookup_labelRepository.GetAll()
                .Select(label => new TaskLabelLabelLookupTableDto
                {
                    Id = label.Id,
                    DisplayName = label == null || label.Name == null ? "" : label.Name.ToString()
                }).ToListAsync();
        }

    }
}