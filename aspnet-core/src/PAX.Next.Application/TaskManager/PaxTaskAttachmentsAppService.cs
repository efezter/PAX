
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PAX.Next.Authorization;
using PAX.Next.Authorization.Users;
using PAX.Next.TaskManager.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Runtime.Session;
using Abp.Events.Bus.Entities;

namespace PAX.Next.TaskManager
{
    [AbpAuthorize(AppPermissions.Pages_PaxTaskAttachments)]
    public class PaxTaskAttachmentsAppService : NextAppServiceBase, IPaxTaskAttachmentsAppService
    {
        private readonly IRepository<PaxTaskAttachment> _paxTaskAttachmentRepository;
        private readonly IRepository<PaxTask, int> _lookup_paxTaskRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IHostEnvironment _env;
        private readonly ITaskHistoriesAppService _taskHistoriesAppService;

        public PaxTaskAttachmentsAppService(IRepository<PaxTaskAttachment> paxTaskAttachmentRepository, IRepository<PaxTask, int> lookup_paxTaskRepository, IRepository<User, long> lookup_userRepository, IHostEnvironment env, ITaskHistoriesAppService taskHistoriesAppService)
        {
            _paxTaskAttachmentRepository = paxTaskAttachmentRepository;
            _lookup_paxTaskRepository = lookup_paxTaskRepository;
            _lookup_userRepository = lookup_userRepository;
            _taskHistoriesAppService = taskHistoriesAppService;
            _env = env;
        }

        public async Task<PagedResultDto<PaxTaskAttachmentDto>> GetAll(GetAllPaxTaskAttachmentsInput input)
        {

            var filteredPaxTaskAttachments = _paxTaskAttachmentRepository.GetAll()
                        .Include(e => e.PaxTaskFk)

                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FileName == input.Filter)
                        .WhereIf(input.TaskId != 0, e => e.PaxTaskFk != null && e.PaxTaskFk.Id == input.TaskId);

            var pagedAndFilteredPaxTaskAttachments = filteredPaxTaskAttachments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var paxTaskAttachments = from o in pagedAndFilteredPaxTaskAttachments
                                     join o2 in _lookup_userRepository.GetAll() on o.CreatorUserId equals o2.Id into j2
                                     from s2 in j2.DefaultIfEmpty()
                                     select new
                                     {
                                         Id = o.Id,
                                         FileName = o.FileName,
                                         CreationTime = o.CreationTime,
                                         Username = s2.FullName
                                     };

            var totalCount = await filteredPaxTaskAttachments.CountAsync();

            var dbList = await paxTaskAttachments.ToListAsync();
            var results = new List<PaxTaskAttachmentDto>();

            foreach (var o in dbList)
            {
                var res = new PaxTaskAttachmentDto()
                {
                        Id = o.Id,
                        FileName = o.FileName,
                        FileUrl = AbpSession.TenantId + "/Tasks/" + input.TaskId.ToString() + "/Attachments/" + o.FileName,
                        CreationTime = o.CreationTime,
                        UserName = o.Username
                };

                results.Add(res);
            }

            return new PagedResultDto<PaxTaskAttachmentDto>(
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

            paxTaskAttachment.Id = await _paxTaskAttachmentRepository.InsertAndGetIdAsync(paxTaskAttachment);
            //await CurrentUnitOfWork.SaveChangesAsync();

            CreateOrEditTaskHistoryDto historyDto = new CreateOrEditTaskHistoryDto { PaxTaskId = input.PaxTaskId, FieldName = "Attachments", CreatedUser = AbpSession.GetUserId(), ChangeType = EntityChangeType.Created, NewValue = paxTaskAttachment.Id.ToString(), CreatedDate = DateTime.Now };
            await _taskHistoriesAppService.CreateOrEdit(historyDto);

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
            var atchRec = _paxTaskAttachmentRepository.Get(input.Id);

            if (atchRec != null)
            {
                var path = "wwwroot/" + AbpSession.TenantId + "/Tasks/" + atchRec.PaxTaskId.ToString() + "/Attachments/" + atchRec.FileName;
                var dir = Path.Combine(_env.ContentRootPath, path);

                if (File.Exists(dir))
                {
                    File.Delete(dir);
                } 
            }

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