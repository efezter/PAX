using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using PAX.Next.TaskManager.Dtos;
using PAX.Next.Dto;
using System.Collections.Generic;

namespace PAX.Next.TaskManager
{
    public interface IPaxTasksAppService : IApplicationService
    {
        Task<PagedResultDto<GetPaxTaskForViewDto>> GetAll(GetAllPaxTasksInput input);

        Task<GetPaxTaskForViewDto> GetPaxTaskForView(int id);

        Task<GetPaxTaskForEditOutput> GetPaxTaskForEdit(EntityDto input);

        Task<NameValueDto> CreateOrEdit(CreateOrEditPaxTaskDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetPaxTasksToExcel(GetAllPaxTasksForExcelInput input);

        Task<PagedResultDto<PaxTaskUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

        Task<List<PaxTaskSeverityLookupTableDto>> GetAllSeverityForTableDropdown();

        Task<List<PaxTaskTaskStatusLookupTableDto>> GetAllTaskStatusForTableDropdown();

    }
}