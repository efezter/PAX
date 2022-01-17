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
    public interface ITaskHistoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetTaskHistoryForViewDto>> GetAll(GetAllTaskHistoriesInput input);

        Task<GetTaskHistoryForEditOutput> GetTaskHistoryForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTaskHistoryDto input);

        Task Delete(EntityDto input);

        Task<List<TaskHistoryPaxTaskLookupTableDto>> GetAllPaxTaskForTableDropdown();

        Task<List<TaskHistoryUserLookupTableDto>> GetAllUserForTableDropdown();

    }
}