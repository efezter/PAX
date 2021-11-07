using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using PAX.Next.TaskManager.Exporting;
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
    [AbpAuthorize(AppPermissions.Pages_Severities)]
    public class SeveritiesAppService : NextAppServiceBase, ISeveritiesAppService
    {
        private readonly IRepository<Severity> _severityRepository;
        private readonly ISeveritiesExcelExporter _severitiesExcelExporter;

        public SeveritiesAppService(IRepository<Severity> severityRepository, ISeveritiesExcelExporter severitiesExcelExporter)
        {
            _severityRepository = severityRepository;
            _severitiesExcelExporter = severitiesExcelExporter;

        }

        public async Task<PagedResultDto<GetSeverityForViewDto>> GetAll(GetAllSeveritiesInput input)
        {

            var filteredSeverities = _severityRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter)
                        .WhereIf(input.MinInsertedDateFilter != null, e => e.InsertedDate >= input.MinInsertedDateFilter)
                        .WhereIf(input.MaxInsertedDateFilter != null, e => e.InsertedDate <= input.MaxInsertedDateFilter);

            var pagedAndFilteredSeverities = filteredSeverities
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var severities = from o in pagedAndFilteredSeverities
                             select new
                             {

                                 o.Name,
                                 o.Order,
                                 o.InsertedDate,
                                 Id = o.Id
                             };

            var totalCount = await filteredSeverities.CountAsync();

            var dbList = await severities.ToListAsync();
            var results = new List<GetSeverityForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSeverityForViewDto()
                {
                    Severity = new SeverityDto
                    {

                        Name = o.Name,
                        Order = o.Order,
                        InsertedDate = o.InsertedDate,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetSeverityForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetSeverityForViewDto> GetSeverityForView(int id)
        {
            var severity = await _severityRepository.GetAsync(id);

            var output = new GetSeverityForViewDto { Severity = ObjectMapper.Map<SeverityDto>(severity) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Severities_Edit)]
        public async Task<GetSeverityForEditOutput> GetSeverityForEdit(EntityDto input)
        {
            var severity = await _severityRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSeverityForEditOutput { Severity = ObjectMapper.Map<CreateOrEditSeverityDto>(severity) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSeverityDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Severities_Create)]
        protected virtual async Task Create(CreateOrEditSeverityDto input)
        {
            var severity = ObjectMapper.Map<Severity>(input);

            await _severityRepository.InsertAsync(severity);

        }

        [AbpAuthorize(AppPermissions.Pages_Severities_Edit)]
        protected virtual async Task Update(CreateOrEditSeverityDto input)
        {
            var severity = await _severityRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, severity);

        }

        [AbpAuthorize(AppPermissions.Pages_Severities_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _severityRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetSeveritiesToExcel(GetAllSeveritiesForExcelInput input)
        {

            var filteredSeverities = _severityRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter)
                        .WhereIf(input.MinInsertedDateFilter != null, e => e.InsertedDate >= input.MinInsertedDateFilter)
                        .WhereIf(input.MaxInsertedDateFilter != null, e => e.InsertedDate <= input.MaxInsertedDateFilter);

            var query = (from o in filteredSeverities
                         select new GetSeverityForViewDto()
                         {
                             Severity = new SeverityDto
                             {
                                 Name = o.Name,
                                 Order = o.Order,
                                 InsertedDate = o.InsertedDate,
                                 Id = o.Id
                             }
                         });

            var severityListDtos = await query.ToListAsync();

            return _severitiesExcelExporter.ExportToFile(severityListDtos);
        }

    }
}