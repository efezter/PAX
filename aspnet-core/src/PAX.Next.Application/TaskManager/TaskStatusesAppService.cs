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
using Abp.Runtime.Session;
using Microsoft.Extensions.Hosting;

namespace PAX.Next.TaskManager
{
    [AbpAuthorize(AppPermissions.Pages_TaskStatuses)]
    public class TaskStatusesAppService : NextAppServiceBase, ITaskStatusesAppService
    {
        private readonly IRepository<TaskStatus> _taskStatusRepository;
        private readonly ITaskStatusesExcelExporter _taskStatusesExcelExporter;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IHostEnvironment _env;

        public TaskStatusesAppService(IRepository<TaskStatus> taskStatusRepository, ITaskStatusesExcelExporter taskStatusesExcelExporter, ITempFileCacheManager tempFileCacheManager, IBinaryObjectManager binaryObjectManager, IHostEnvironment env)
        {
            _taskStatusRepository = taskStatusRepository;
            _taskStatusesExcelExporter = taskStatusesExcelExporter;
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
            _env = env;
        }

        public async Task<PagedResultDto<GetTaskStatusForViewDto>> GetAll(GetAllTaskStatusesInput input)
        {

            var filteredTaskStatuses = _taskStatusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var pagedAndFilteredTaskStatuses = filteredTaskStatuses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var taskStatuses = from o in pagedAndFilteredTaskStatuses
                               select new
                               {

                                   o.Name,
                                   o.IconUrl,
                                   Id = o.Id
                               };

            var totalCount = await filteredTaskStatuses.CountAsync();

            var dbList = await taskStatuses.ToListAsync();
            var results = new List<GetTaskStatusForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTaskStatusForViewDto()
                {
                    TaskStatus = new TaskStatusDto
                    {

                        Name = o.Name,
                        IconUrl = o.IconUrl,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTaskStatusForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetTaskStatusForViewDto> GetTaskStatusForView(int id)
        {
            var taskStatus = await _taskStatusRepository.GetAsync(id);

            var output = new GetTaskStatusForViewDto { TaskStatus = ObjectMapper.Map<TaskStatusDto>(taskStatus) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TaskStatuses_Edit)]
        public async Task<GetTaskStatusForEditOutput> GetTaskStatusForEdit(EntityDto input)
        {
            var taskStatus = await _taskStatusRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTaskStatusForEditOutput { TaskStatus = ObjectMapper.Map<CreateOrEditTaskStatusDto>(taskStatus) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTaskStatusDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TaskStatuses_Create)]
        protected virtual async Task Create(CreateOrEditTaskStatusDto input)
        {
            var taskStatus = ObjectMapper.Map<TaskStatus>(input);

            await _taskStatusRepository.InsertAsync(taskStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskStatuses_Edit)]
        protected virtual async Task Update(CreateOrEditTaskStatusDto input)
        {
            var taskStatus = await _taskStatusRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, taskStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskStatuses_Delete)]
        public async Task Delete(EntityDto input)
        {
            await DeleteIcon(input.Id);
            await _taskStatusRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTaskStatusesToExcel(GetAllTaskStatusesForExcelInput input)
        {

            var filteredTaskStatuses = _taskStatusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var query = (from o in filteredTaskStatuses
                         select new GetTaskStatusForViewDto()
                         {
                             TaskStatus = new TaskStatusDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var taskStatusListDtos = await query.ToListAsync();

            return _taskStatusesExcelExporter.ExportToFile(taskStatusListDtos);
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

            var iconUrl = "wwwroot/Common/Images/" + AbpSession.TenantId + "/Task/TaskStatusIcons";

            var dir = Path.Combine(_env.ContentRootPath, iconUrl);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var filePath = Path.Combine(dir, input.TaskStatusId + ".png");

            File.WriteAllBytes(filePath, byteArray);

            iconUrl = "/" + iconUrl.Replace("wwwroot/","") + "/" + input.TaskStatusId + ".png";

            SaveImagePathToDb(input.TaskStatusId, iconUrl);

        }

        public async Task DeleteIcon(int statusId)
        {
            var taskStatus = await _taskStatusRepository.GetAsync(statusId);

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

        private void SaveImagePathToDb(int taskStatusId, string iconUrl)
        {
            var taskStatus =  _taskStatusRepository.FirstOrDefault(taskStatusId);
            CreateOrEditTaskStatusDto input = new CreateOrEditTaskStatusDto { Id = taskStatusId,Name = taskStatus.Name, IconUrl = iconUrl};
            ObjectMapper.Map(input, taskStatus);
        }
    }
}