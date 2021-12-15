using PAX.Next.TaskManager;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using PAX.Next.TaskManager.Dtos;
using PAX.Next.Dto;
using Abp.Application.Services.Dto;
using PAX.Next.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using PAX.Next.Storage;

namespace PAX.Next.TaskManager
{
    [AbpAuthorize(AppPermissions.Pages_PaxTaskAttachments)]
    public class PaxTaskAttachmentsAppService : NextAppServiceBase, IPaxTaskAttachmentsAppService
    {
        private readonly IRepository<PaxTaskAttachment> _paxTaskAttachmentRepository;
        private readonly IRepository<PaxTask, int> _lookup_paxTaskRepository;

        public PaxTaskAttachmentsAppService(IRepository<PaxTaskAttachment> paxTaskAttachmentRepository, IRepository<PaxTask, int> lookup_paxTaskRepository)
        {
            _paxTaskAttachmentRepository = paxTaskAttachmentRepository;
            _lookup_paxTaskRepository = lookup_paxTaskRepository;

        }

        public async Task<PagedResultDto<GetPaxTaskAttachmentForViewDto>> GetAll(GetAllPaxTaskAttachmentsInput input)
        {

            var filteredPaxTaskAttachments = _paxTaskAttachmentRepository.GetAll()
                        .Include(e => e.PaxTaskFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FileName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaxTaskHeaderFilter), e => e.PaxTaskFk != null && e.PaxTaskFk.Header == input.PaxTaskHeaderFilter);

            var pagedAndFilteredPaxTaskAttachments = filteredPaxTaskAttachments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var paxTaskAttachments = from o in pagedAndFilteredPaxTaskAttachments
                                     join o1 in _lookup_paxTaskRepository.GetAll() on o.PaxTaskId equals o1.Id into j1
                                     from s1 in j1.DefaultIfEmpty()

                                     select new
                                     {

                                         Id = o.Id,
                                         PaxTaskHeader = s1 == null || s1.Header == null ? "" : s1.Header.ToString()
                                     };

            var totalCount = await filteredPaxTaskAttachments.CountAsync();

            var dbList = await paxTaskAttachments.ToListAsync();
            var results = new List<GetPaxTaskAttachmentForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPaxTaskAttachmentForViewDto()
                {
                    PaxTaskAttachment = new PaxTaskAttachmentDto
                    {

                        Id = o.Id,
                    },
                    PaxTaskHeader = o.PaxTaskHeader
                };

                results.Add(res);
            }

            return new PagedResultDto<GetPaxTaskAttachmentForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_PaxTaskAttachments_Edit)]
        public async Task<GetPaxTaskAttachmentForEditOutput> GetPaxTaskAttachmentForEdit(EntityDto input)
        {
            var paxTaskAttachment = await _paxTaskAttachmentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPaxTaskAttachmentForEditOutput { PaxTaskAttachment = ObjectMapper.Map<CreateOrEditPaxTaskAttachmentDto>(paxTaskAttachment) };

            if (output.PaxTaskAttachment.PaxTaskId != null)
            {
                var _lookupPaxTask = await _lookup_paxTaskRepository.FirstOrDefaultAsync((int)output.PaxTaskAttachment.PaxTaskId);
                output.PaxTaskHeader = _lookupPaxTask?.Header?.ToString();
            }

            return output;
        }

        public async Task<int> CreateOrEdit(CreateOrEditPaxTaskAttachmentDto input)
        {
            if (input.Id == null)
            {
                return await Create(input);
            }
            else
            {
               return await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_PaxTaskAttachments_Create)]
        protected virtual async Task<int> Create(CreateOrEditPaxTaskAttachmentDto input)
        {
            var paxTaskAttachment = ObjectMapper.Map<PaxTaskAttachment>(input);

            await _paxTaskAttachmentRepository.InsertAsync(paxTaskAttachment);
            await CurrentUnitOfWork.SaveChangesAsync();

            return paxTaskAttachment.Id;

        }

        [AbpAuthorize(AppPermissions.Pages_PaxTaskAttachments_Edit)]
        protected virtual async Task<int> Update(CreateOrEditPaxTaskAttachmentDto input)
        {
            var paxTaskAttachment = await _paxTaskAttachmentRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, paxTaskAttachment);
            return input.Id.Value;
        }

        [AbpAuthorize(AppPermissions.Pages_PaxTaskAttachments_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _paxTaskAttachmentRepository.DeleteAsync(input.Id);
        }
        [AbpAuthorize(AppPermissions.Pages_PaxTaskAttachments)]
        public async Task<List<PaxTaskAttachmentPaxTaskLookupTableDto>> GetAllPaxTaskForTableDropdown()
        {
            return await _lookup_paxTaskRepository.GetAll()
                .Select(paxTask => new PaxTaskAttachmentPaxTaskLookupTableDto
                {
                    Id = paxTask.Id,
                    DisplayName = paxTask == null || paxTask.Header == null ? "" : paxTask.Header.ToString()
                }).ToListAsync();
        }

    }
}