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
    public interface ITaskDependancyRelationsAppService : IApplicationService
    {
        Task<PagedResultDto<GetTaskDependancyRelationForViewDto>> GetAll(GetAllTaskDependancyRelationsInput input);

        Task<GetTaskDependancyRelationForViewDto> GetTaskDependancyRelationForView(int id);

        Task<GetTaskDependancyRelationForEditOutput> GetTaskDependancyRelationForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTaskDependancyRelationDto input);

        Task Delete(int input);

        Task<List<TaskDependancyRelationPaxTaskLookupTableDto>> GetAllPaxTaskForTableDropdown();

        Task<List<TaskDependancyRelationDto>> GetTasksDependecies(int taskId);

    }
}