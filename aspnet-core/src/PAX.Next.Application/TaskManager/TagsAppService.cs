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
    [AbpAuthorize(AppPermissions.Pages_Tags)]
    public class TagsAppService : NextAppServiceBase, ITagsAppService
    {
        private readonly IRepository<Tag> _tagRepository;

        public TagsAppService(IRepository<Tag> tagRepository)
        {
            _tagRepository = tagRepository;

        }

        public async Task<PagedResultDto<GetTagForViewDto>> GetAll(GetAllTagsInput input)
        {

            var filteredTags = _tagRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter));

            var pagedAndFilteredTags = filteredTags
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var tags = from o in pagedAndFilteredTags
                       select new
                       {

                           Id = o.Id
                       };

            var totalCount = await filteredTags.CountAsync();

            var dbList = await tags.ToListAsync();
            var results = new List<GetTagForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTagForViewDto()
                {
                    Tag = new TagDto
                    {

                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTagForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetTagForViewDto> GetTagForView(int id)
        {
            var tag = await _tagRepository.GetAsync(id);

            var output = new GetTagForViewDto { Tag = ObjectMapper.Map<TagDto>(tag) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Tags_Edit)]
        public async Task<GetTagForEditOutput> GetTagForEdit(EntityDto input)
        {
            var tag = await _tagRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTagForEditOutput { Tag = ObjectMapper.Map<CreateOrEditTagDto>(tag) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTagDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Tags_Create)]
        protected virtual async Task Create(CreateOrEditTagDto input)
        {
            var tag = ObjectMapper.Map<Tag>(input);

            await _tagRepository.InsertAsync(tag);

        }

        [AbpAuthorize(AppPermissions.Pages_Tags_Edit)]
        protected virtual async Task Update(CreateOrEditTagDto input)
        {
            var tag = await _tagRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, tag);

        }

        [AbpAuthorize(AppPermissions.Pages_Tags_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _tagRepository.DeleteAsync(input.Id);
        }

    }
}