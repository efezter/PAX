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
using System.Drawing;
using System.IO;
using Microsoft.Extensions.Hosting;

namespace PAX.Next.TaskManager
{
    [AbpAuthorize(AppPermissions.Pages_Severities)]
    public class SeveritiesAppService : NextAppServiceBase, ISeveritiesAppService
    {
        private readonly IRepository<Severity> _severityRepository;
        private readonly ISeveritiesExcelExporter _severitiesExcelExporter;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IHostEnvironment _env;

        public SeveritiesAppService(IRepository<Severity> severityRepository, ISeveritiesExcelExporter severitiesExcelExporter, ITempFileCacheManager tempFileCacheManager, IHostEnvironment env)
        {
            _severityRepository = severityRepository;
            _severitiesExcelExporter = severitiesExcelExporter;
            _tempFileCacheManager = tempFileCacheManager;
            _env = env;

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
                                 o.IconUrl,
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
                        IconUrl = o.IconUrl,
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
            await DeleteIcon(input.Id);
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

        /// <summary>
        /// Updates Icon of task status
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateIcon(UpdateIconInput input)
        {
            byte[] byteArray;

            var imageBytes = _tempFileCacheManager.GetFile(input.FileToken);

            if (imageBytes == null)
            {
                throw new UserFriendlyException("There is no such image file with the token: " + input.FileToken);
            }

            using (var bmpImage = new Bitmap(new MemoryStream(imageBytes)))
            {
                var width = (input.Width == 0 || input.Width > bmpImage.Width) ? bmpImage.Width : input.Width;
                var height = (input.Height == 0 || input.Height > bmpImage.Height) ? bmpImage.Height : input.Height;
                var bmCrop = bmpImage.Clone(new Rectangle(input.X, input.Y, width, height), bmpImage.PixelFormat);

                using (var stream = new MemoryStream())
                {
                    bmCrop.Save(stream, bmpImage.RawFormat);
                    byteArray = stream.ToArray();
                }
            }

            if (byteArray.Length > AppConsts.MaxIconBytes)
            {
                throw new UserFriendlyException(L("ResizedProfilePicture_Warn_SizeLimit",
                    AppConsts.ResizedMaxIconBytesUserFriendlyValue));
            }

            var iconUrl = "wwwroot/Common/Images/" + AbpSession.TenantId + "/Task/SeverityIcons";

            var dir = Path.Combine(_env.ContentRootPath, iconUrl);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var filePath = Path.Combine(dir, input.TaskStatusId + ".png");

            File.WriteAllBytes(filePath, byteArray);

            iconUrl = "/" + iconUrl.Replace("wwwroot/", "") + "/" + input.TaskStatusId + ".png";

            SaveImagePathToDb(input.TaskStatusId, iconUrl);

        }

        public async Task DeleteIcon(int severityId)
        {
            var taskStatus = await _severityRepository.GetAsync(severityId);

            if (!string.IsNullOrEmpty(taskStatus.IconUrl))
            {
                var path = "wwwroot" + taskStatus.IconUrl;
                var dir = Path.Combine(_env.ContentRootPath, path);

                if (File.Exists(dir))
                {
                    File.Delete(dir);
                }
            }
        }

        private void SaveImagePathToDb(int severityId, string iconUrl)
        {
            var severity = _severityRepository.FirstOrDefault(severityId);
            CreateOrEditSeverityDto input = new CreateOrEditSeverityDto { Id = severityId, Name = severity.Name, IconUrl = iconUrl };
            ObjectMapper.Map(input, severity);
        }

    }
}