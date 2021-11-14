using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using PAX.Next.TaskManager.Dtos;
using PAX.Next.Dto;

namespace PAX.Next.TaskManager
{
    public interface ITagsAppService : IApplicationService
    {
        Task<PagedResultDto<GetTagForViewDto>> GetAll(GetAllTagsInput input);

        Task<GetTagForViewDto> GetTagForView(int id);

        Task<GetTagForEditOutput> GetTagForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTagDto input);

        Task Delete(EntityDto input);

    }
}