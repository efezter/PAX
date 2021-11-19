using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using PAX.Next.TaskManager.Dtos;
using PAX.Next.Dto;

namespace PAX.Next.TaskManager
{
    public interface ICommentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetCommentForViewDto>> GetAll(GetAllCommentsInput input);

        Task<GetCommentForEditOutput> GetCommentForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCommentDto input);

        Task Delete(EntityDto input);

        Task<PagedResultDto<CommentPaxTaskLookupTableDto>> GetAllPaxTaskForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<CommentUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

    }
}