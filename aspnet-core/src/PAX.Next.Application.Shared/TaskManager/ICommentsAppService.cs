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

        Task<CommentDto> CreateOrEdit(CreateOrEditCommentDto input);

        Task Delete(EntityDto input);

    }
}