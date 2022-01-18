using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using PAX.Next.TaskManager.Dtos;
using PAX.Next.Dto;
using System.Collections.Generic;
using System.Collections.Generic;

namespace PAX.Next.TaskManager
{
    public interface ITaskLabelsAppService : IApplicationService
    {
        Task<PagedResultDto<GetTaskLabelForViewDto>> GetAll(GetAllTaskLabelsInput input);

        Task<GetTaskLabelForEditOutput> GetTaskLabelForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTaskLabelDto input);

        Task Delete(int Id);

        Task<List<TaskLabelPaxTaskLookupTableDto>> GetAllPaxTaskForTableDropdown();

        Task<List<TaskLabelLabelLookupTableDto>> GetAllLabelForTableDropdown();

    }
}