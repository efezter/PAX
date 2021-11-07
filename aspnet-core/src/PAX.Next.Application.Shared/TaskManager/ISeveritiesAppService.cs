using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using PAX.Next.TaskManager.Dtos;
using PAX.Next.Dto;

namespace PAX.Next.TaskManager
{
    public interface ISeveritiesAppService : IApplicationService
    {
        Task<PagedResultDto<GetSeverityForViewDto>> GetAll(GetAllSeveritiesInput input);

        Task<GetSeverityForViewDto> GetSeverityForView(int id);

        Task<GetSeverityForEditOutput> GetSeverityForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditSeverityDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetSeveritiesToExcel(GetAllSeveritiesForExcelInput input);

    }
}