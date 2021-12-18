using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using PAX.Next.TaskManager.Dtos;
using PAX.Next.Dto;
using System.Collections.Generic;

namespace PAX.Next.TaskManager
{
    public interface IPaxTaskAttachmentsAppService : IApplicationService
    {
        Task<PagedResultDto<PaxTaskAttachmentDto>> GetAll(GetAllPaxTaskAttachmentsInput input);

        Task<GetPaxTaskAttachmentForEditOutput> GetPaxTaskAttachmentForEdit(EntityDto input);

        Task<int> CreateOrEdit(CreateOrEditPaxTaskAttachmentDto input);

        Task Delete(EntityDto input);

        Task<List<PaxTaskAttachmentPaxTaskLookupTableDto>> GetAllPaxTaskForTableDropdown();

    }
}