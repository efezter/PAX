using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using PAX.Next.Authorization;
using PAX.Next.TaskManager.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace PAX.Next.TaskManager
{
    [AbpAuthorize(AppPermissions.Pages_Labels)]
    public class LabelsAppService : NextAppServiceBase, ILabelsAppService
    {
        private readonly IRepository<Label> _labelRepository;
        private readonly IRepository<TaskLabel> _taskLabelRepository;

        public LabelsAppService(IRepository<Label> labelRepository, IRepository<TaskLabel> taskLabelRepository)
        {
            _labelRepository = labelRepository;
            _taskLabelRepository = taskLabelRepository;

        }

        public async Task<PagedResultDto<GetLabelForViewDto>> GetAll(GetAllLabelsInput input)
        {

            var filteredLabels = _labelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter));

            var pagedAndFilteredLabels = filteredLabels
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var labels = from o in pagedAndFilteredLabels
                         select new
                         {
                             Id = o.Id,
                             Name = o.Name
                         };

            var totalCount = await filteredLabels.CountAsync();

            var dbList = await labels.ToListAsync();
            var results = new List<GetLabelForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetLabelForViewDto()
                {
                    Label = new LabelDto
                    {
                        Id = o.Id,
                        Name = o.Name
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetLabelForViewDto>(
                totalCount,
                results
            );
        }

        public IQueryable<LabelDto> GetLabelsByTaskId(int taskId)
        {
            var labels = from w in _taskLabelRepository.GetAll()
                         join o2 in _labelRepository.GetAll() on w.LabelId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         where w.PaxTaskId == taskId
                         select new LabelDto
                         {
                             TaskLabelId = w.Id,
                             Id = s2.Id,
                             Name = s2.Name
                         };

            return labels;
        }


        [AbpAuthorize(AppPermissions.Pages_Labels_Edit)]
        public async Task<GetLabelForEditOutput> GetLabelForEdit(EntityDto input)
        {
            var label = await _labelRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLabelForEditOutput { Label = ObjectMapper.Map<CreateOrEditLabelDto>(label) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditLabelDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Labels_Create)]
        protected virtual async Task Create(CreateOrEditLabelDto input)
        {
            var label = ObjectMapper.Map<Label>(input);

            await _labelRepository.InsertAsync(label);

        }

        [AbpAuthorize(AppPermissions.Pages_Labels_Edit)]
        protected virtual async Task Update(CreateOrEditLabelDto input)
        {
            var label = await _labelRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, label);

        }

        [AbpAuthorize(AppPermissions.Pages_Labels_Delete)]
        public async Task Delete(int id)
        {
            await _labelRepository.DeleteAsync(id);
        }

    }
}