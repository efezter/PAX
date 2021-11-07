using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using PAX.Next.TaskManager.Dtos;
using PAX.Next.Dto;

namespace PAX.Next.TaskManager
{
    public interface ITaskStatusesAppService : IApplicationService
    {
        Task<PagedResultDto<GetTaskStatusForViewDto>> GetAll(GetAllTaskStatusesInput input);

        Task<GetTaskStatusForViewDto> GetTaskStatusForView(int id);

        Task<GetTaskStatusForEditOutput> GetTaskStatusForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTaskStatusDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetTaskStatusesToExcel(GetAllTaskStatusesForExcelInput input);

    }
}