using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityHistory;
using Abp.Events.Bus.Entities;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using PAX.Next.Authorization;
using PAX.Next.Authorization.Users;
using PAX.Next.Dto;
using PAX.Next.Notifications;
using PAX.Next.Organizations;
using PAX.Next.Organizations.Dto;
using PAX.Next.TaskManager.Dtos;
using PAX.Next.TaskManager.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static PAX.Next.TaskManager.Utils.Enums;

namespace PAX.Next.TaskManager
{
    [AbpAuthorize(AppPermissions.Pages_PaxTasks)]
    public class PaxTasksAppService : NextAppServiceBase, IPaxTasksAppService
    {
        private readonly IRepository<PaxTask> _paxTaskRepository;
        private readonly IWatchersAppService _watcherRepository;
        private readonly ITaskDependancyRelationsAppService _taskDependancyRelationsAppService;
        private readonly ITaskHistoriesAppService _taskHistoriesAppService;
        private readonly IPaxTasksExcelExporter _paxTasksExcelExporter;
        private readonly IOrganizationUnitAppService _organizationUnitAppService;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<Severity, int> _lookup_severityRepository;
        private readonly IRepository<TaskStatus, int> _lookup_taskStatusRepository;
        private readonly IRepository<EntityChange, long> _entityChangeRepository;
        private readonly IRepository<EntityChangeSet, long> _entityChangeSetRepository;
        private readonly IRepository<EntityPropertyChange, long> _entityPropertyChangeRepository;
        private readonly ILocalizationSource _localizationSource;
        private readonly IRepository<PaxTaskAttachment> _paxTaskAttachmentRepository;
        private readonly IRepository<Label> _labelRepository;
        private readonly ILabelsAppService _labelService;
        private readonly ITaskLabelsAppService _taskLabelService;
        private readonly IAppNotifier _appNotifier;


        public PaxTasksAppService(
            IRepository<PaxTask> paxTaskRepository,
            IPaxTasksExcelExporter paxTasksExcelExporter,
            IRepository<User, long> lookup_userRepository,
            ITaskHistoriesAppService taskHistoriesAppService,
            IRepository<Severity, int> lookup_severityRepository,
            IRepository<TaskStatus, int> lookup_taskStatusRepository,
            IWatchersAppService watcherRepository,
            IRepository<EntityChange, long> entityChangeRepository,
            IRepository<EntityChangeSet, long> entityChangeSetRepository,
            IRepository<EntityPropertyChange, long> entityPropertyChangeRepository,
            ILocalizationManager localizationManager,
            IRepository<PaxTaskAttachment> paxTaskAttachmentRepository,
            IRepository<Label> labelRepository,
            ILabelsAppService labelService,
            ITaskLabelsAppService taskLabelService,
            IAppNotifier appNotifier,
            ITaskDependancyRelationsAppService taskDependancyRelationsAppService,
            IOrganizationUnitAppService organizationUnitAppService
            )
        {
            _paxTaskRepository = paxTaskRepository;
            _paxTasksExcelExporter = paxTasksExcelExporter;
            _lookup_userRepository = lookup_userRepository;
            _lookup_severityRepository = lookup_severityRepository;
            _lookup_taskStatusRepository = lookup_taskStatusRepository;
            _watcherRepository = watcherRepository;
            _taskHistoriesAppService = taskHistoriesAppService;
            _entityChangeRepository = entityChangeRepository;
            _entityChangeSetRepository = entityChangeSetRepository;
            _entityPropertyChangeRepository = entityPropertyChangeRepository;
            _paxTaskAttachmentRepository = paxTaskAttachmentRepository;
            _labelRepository = labelRepository;
            _labelService = labelService;
            _taskLabelService = taskLabelService;
            _appNotifier = appNotifier;
            _taskDependancyRelationsAppService = taskDependancyRelationsAppService;
            _organizationUnitAppService = organizationUnitAppService;

            _localizationSource = localizationManager.GetSource(NextConsts.LocalizationSourceName);
        }

        public async Task<PagedResultDto<GetPaxTaskForViewDto>> GetAll(GetAllPaxTasksInput input)
        {
            var taskTypeFilter = input.TaskTypeFilter.HasValue
                        ? (TaskType)input.TaskTypeFilter
                        : default;
            var taskTypePeriodFilter = input.TaskTypePeriodFilter.HasValue
                ? (TaskTypePeriod)input.TaskTypePeriodFilter
                : default;

            var currentUserId = AbpSession.GetUserId();
            User user = new User();
            user.Id = currentUserId;

           bool isAdmin = await UserManager.IsInRoleAsync(user, "Admin");

            if (!isAdmin)
            {
                FindOrganizationUnitUsersInput orgInput = new FindOrganizationUnitUsersInput();

                //orgInput.OrganizationUnitId = UserManager.ur 
                //_organizationUnitAppService.FindUsers()
            }

            var filteredPaxTasks = _paxTaskRepository.GetAll()
                        .Include(e => e.ReporterFk)
                        .Include(e => e.AssigneeFk)
                        .Include(e => e.SeverityFk)
                        .Include(e => e.TaskStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Header.Contains(input.Filter) || e.Details.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HeaderFilter), e => e.Header.Contains(input.HeaderFilter))
                        .WhereIf(input.MinCreatedDateFilter != null, e => e.CreatedDate >= input.MinCreatedDateFilter)
                        .WhereIf(input.MaxCreatedDateFilter != null, e => e.CreatedDate <= input.MaxCreatedDateFilter)
                        .WhereIf(input.TaskTypeFilter.HasValue && input.TaskTypeFilter > -1, e => e.TaskType == taskTypeFilter)
                        .WhereIf(input.TaskTypePeriodFilter.HasValue && input.TaskTypePeriodFilter > -1, e => e.TaskTypePeriod == taskTypePeriodFilter)
                        .WhereIf(input.MinPeriodIntervalFilter != null, e => e.PeriodInterval >= input.MinPeriodIntervalFilter)
                        .WhereIf(input.MaxPeriodIntervalFilter != null, e => e.PeriodInterval <= input.MaxPeriodIntervalFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.ReporterFk != null && e.ReporterFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.AssigneeFk != null && e.AssigneeFk.Name == input.UserName2Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SeverityNameFilter), e => e.SeverityFk != null && e.SeverityFk.Name == input.SeverityNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskStatusNameFilter), e => e.TaskStatusFk != null && e.TaskStatusFk.Name == input.TaskStatusNameFilter)
                        .WhereIf(input.ShowOnlyCreatedByMe, e => e.ReporterId == currentUserId)
                        .WhereIf(input.ShowOnlyMyTasks, e => e.AssigneeId == currentUserId);
                        //.WhereIf(!isAdmin, e => (e.AssigneeFk != null && e.ReporterFk != null) && e.AssigneeFk == currentUserId);

            var pagedAndFilteredPaxTasks = filteredPaxTasks
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var paxTasks = from o in pagedAndFilteredPaxTasks
                           join o1 in _lookup_userRepository.GetAll() on o.ReporterId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           join o2 in _lookup_userRepository.GetAll() on o.AssigneeId equals o2.Id into j2
                           from s2 in j2.DefaultIfEmpty()

                           join o3 in _lookup_severityRepository.GetAll() on o.SeverityId equals o3.Id into j3
                           from s3 in j3.DefaultIfEmpty()

                           join o4 in _lookup_taskStatusRepository.GetAll() on o.TaskStatusId equals o4.Id into j4
                           from s4 in j4.DefaultIfEmpty()

                           select new
                           {
                               o.Header,
                               o.CreatedDate,
                               o.TaskType,
                               o.TaskTypePeriod,
                               o.PeriodInterval,
                               o.DeadLineDate,
                               Id = o.Id,
                               UserName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                               UserName2 = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString(),
                               SeverityName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                               SeverityIconUrl = s4 == null || s3.IconUrl == null ? "" : s3.IconUrl.ToString(),
                               StatusImgUrl = s4 == null || s4.IconUrl == null ? "" : s4.IconUrl.ToString(),
                               TaskStatusName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                           };

            var totalCount = await filteredPaxTasks.CountAsync();

            var dbList = await paxTasks.ToListAsync();
            var results = new List<GetPaxTaskForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPaxTaskForViewDto()
                {
                    PaxTask = new PaxTaskDto
                    {

                        Header = o.Header,
                        CreatedDate = o.CreatedDate,
                        TaskType = o.TaskType,
                        TaskTypePeriod = o.TaskTypePeriod,
                        PeriodInterval = o.PeriodInterval,
                        DeadLineDate = o.DeadLineDate,
                        Id = o.Id,
                    },
                    UserName = o.UserName,
                    UserName2 = o.UserName2,
                    SeverityName = o.SeverityName,
                    SeverityImgUrl = o.SeverityIconUrl,
                    TaskStatusName = o.TaskStatusName,
                    StatusImgUrl = o.StatusImgUrl
                };

                results.Add(res);
            }

            return new PagedResultDto<GetPaxTaskForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetPaxTaskForViewDto> GetPaxTaskForView(int id)
        {
            var paxTask = await _paxTaskRepository.GetAsync(id);

            var output = new GetPaxTaskForViewDto { PaxTask = ObjectMapper.Map<PaxTaskDto>(paxTask) };

            if (output.PaxTask.ReporterId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.PaxTask.ReporterId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.PaxTask.AssigneeId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.PaxTask.AssigneeId);
                output.UserName2 = _lookupUser?.Name?.ToString();
            }

            if (output.PaxTask.SeverityId != null)
            {
                var _lookupSeverity = await _lookup_severityRepository.FirstOrDefaultAsync((int)output.PaxTask.SeverityId);
                output.SeverityName = _lookupSeverity?.Name?.ToString();
            }

            if (output.PaxTask.TaskStatusId != null)
            {
                var _lookupTaskStatus = await _lookup_taskStatusRepository.FirstOrDefaultAsync((int)output.PaxTask.TaskStatusId);
                output.TaskStatusName = _lookupTaskStatus?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PaxTasks_Edit)]
        public async Task<GetPaxTaskForEditOutput> GetPaxTaskForEdit(EntityDto input)
        {
            var paxTask = await _paxTaskRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPaxTaskForEditOutput { PaxTask = ObjectMapper.Map<CreateOrEditPaxTaskDto>(paxTask) };

            //TODO : Too much unnecessary call to DB.

            output.PaxTask.Watchers = _watcherRepository.GetUserDetailsByTaskId(output.PaxTask.Id.Value).Result.ToList();

            output.PaxTask.DependentTasks = _taskDependancyRelationsAppService.GetTasksDependecies(output.PaxTask.Id.Value).Result.ToList();

            output.PaxTask.Labels = _labelService.GetLabelsByTaskId(output.PaxTask.Id.Value).Result.ToList();

            if (output.PaxTask.ReporterId != 0)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.PaxTask.ReporterId);
                output.UserName = _lookupUser?.FullName?.ToString();
            }

            if (output.PaxTask.AssigneeId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.PaxTask.AssigneeId);
                output.UserName2 = _lookupUser?.FullName?.ToString();
            }

            if (output.PaxTask.SeverityId != null)
            {
                var _lookupSeverity = await _lookup_severityRepository.FirstOrDefaultAsync((int)output.PaxTask.SeverityId);
                output.SeverityName = _lookupSeverity?.Name?.ToString();
            }

            if (output.PaxTask.TaskStatusId != 0)
            {
                var _lookupTaskStatus = await _lookup_taskStatusRepository.FirstOrDefaultAsync((int)output.PaxTask.TaskStatusId);
                output.TaskStatusName = _lookupTaskStatus?.Name?.ToString();
            }

            return output;
        }

        public async Task<List<TaskDependancyRelationDto>> GetTasksForDepandancyDrop(string filter,int taskId)
        {
            var filteredPaxTasks = _paxTaskRepository.GetAll()
                       .WhereIf(!string.IsNullOrWhiteSpace(filter), e => false || e.Header.Contains(filter))
                       .WhereIf(taskId != 0, e => false || e.Id == taskId);

            return (from o in filteredPaxTasks
                   select new TaskDependancyRelationDto
                   {
                       DependOnHeader = o.Header,
                       DependOnTaskId = o.Id,

                   }).ToList();
        }

        public async Task<NameValueDto> CreateOrEdit(CreateOrEditPaxTaskDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PaxTasks_Create)]
        protected virtual async Task<NameValueDto> Create(CreateOrEditPaxTaskDto input)
        {
            var paxTask = ObjectMapper.Map<PaxTask>(input);

            paxTask.CreatedDate = DateTime.Now;
            paxTask.ReporterId = AbpSession.GetUserId();

            await _paxTaskRepository.InsertAsync(paxTask);
            await CurrentUnitOfWork.SaveChangesAsync();


            if (input.Watchers != null && input.Watchers.Count() > 0)
            {
                List<Task> inserTasks = new List<Task>();
                foreach (var watcher in input.Watchers)
                {
                    CreateOrEditWatcherDto watcherDto = new CreateOrEditWatcherDto { UserId = watcher.UserId, PaxTaskId = paxTask.Id };
                    inserTasks.Add(_watcherRepository.CreateOrEdit(watcherDto));

                    CreateOrEditTaskHistoryDto historyDto = new CreateOrEditTaskHistoryDto { PaxTaskId = paxTask.Id, FieldName = "Watchers", CreatedUser = paxTask.ReporterId, NewValue = watcher.UserId.ToString(), ChangeType = EntityChangeType.Created, CreatedDate = DateTime.Now };
                    inserTasks.Add(_taskHistoriesAppService.CreateOrEdit(historyDto));
                }

                Task.WaitAll(inserTasks.ToArray());
            }

            await UpdateLabels(paxTask.Id, input.Labels.ToList());

            await UpdateDependants(paxTask.Id, input.DependentTasks.ToList());


            string changerUserName = _lookup_userRepository.Get(AbpSession.UserId.Value).FullName;

            List<UserIdentifier> notificators = new List<UserIdentifier> { new UserIdentifier(AbpSession.TenantId, paxTask.AssigneeId.Value) };

            await _appNotifier.TaskChangedAsync(changerUserName, paxTask.Id, "TaskCreatedNotification", notificators.ToArray());

            if (input.Watchers == null)
            {
                input.Watchers = new List<WatcherUserLookupTableDto>();
            }

            notificators = new List<UserIdentifier>();

            foreach (var watcher in input.Watchers)
            {
                notificators.Add(new UserIdentifier(AbpSession.TenantId, watcher.UserId));
            }

            await _appNotifier.TaskChangedAsync(changerUserName, paxTask.Id, "TaskCreatedNotificationWatcher", notificators.ToArray());



            return new NameValueDto("taskId", paxTask.Id.ToString());
        }

        [AbpAuthorize(AppPermissions.Pages_PaxTasks_Edit)]
        protected virtual async Task<NameValueDto> Update(CreateOrEditPaxTaskDto input)
        {
            var paxTask = await _paxTaskRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, paxTask);



            string changerUserName = _lookup_userRepository.Get(AbpSession.UserId.Value).FullName;

            List<UserIdentifier> notificators = new List<UserIdentifier> { new UserIdentifier(AbpSession.TenantId, paxTask.AssigneeId.Value) };

            await _appNotifier.TaskChangedAsync(changerUserName, paxTask.Id, "TaskChangedNotification", notificators.ToArray());           

            if (input.Watchers == null)
            {
                input.Watchers = new List<WatcherUserLookupTableDto>();
            }

            notificators = new List<UserIdentifier>();

            foreach (var watcher in input.Watchers)
            {
                notificators.Add(new UserIdentifier(AbpSession.TenantId, watcher.UserId));
            }

            await _appNotifier.TaskChangedAsync(changerUserName, paxTask.Id, "TaskChangedNotificationWatcher", notificators.ToArray());




            await UpdateWatchers(paxTask.Id, input.Watchers.ToList());
            await UpdateLabels(paxTask.Id, input.Labels.ToList());
            await UpdateDependants(paxTask.Id, input.DependentTasks.ToList());



            return new NameValueDto("taskId", paxTask.Id.ToString());
        }

        //async Task Notify(string changerUserName,IEnumerable<WatcherUserLookupTableDto> watchers, PaxTask paxTask, string messageId)
        //{
        //    List<UserIdentifier> notificators = new List<UserIdentifier> { new UserIdentifier(AbpSession.TenantId, paxTask.AssigneeId.Value) };

        //    if (watchers != null)
        //    {
        //        foreach (var watcher in watchers)
        //        {
        //            notificators.Add(new UserIdentifier(AbpSession.TenantId, watcher.UserId));
        //        }
        //    }
        //     await _appNotifier.TaskChangedAsync(changerUserName, paxTask.Id, messageId, notificators.ToArray());
            
           
        //}

        private async Task UpdateLabels(int taskId, List<LabelDto> updatedLabels)
        {
            var existingLabels = _labelService.GetLabelsByTaskId(taskId).Result.ToList();

            var deletedLabels = existingLabels.Where(x => updatedLabels.Select(w => w.Id).Contains(x.Id) == false).ToList();

            var currentUserId = AbpSession.GetUserId();

            if (deletedLabels != null && deletedLabels.Count() > 0)
            {
                List<Task> delTasks = new List<Task>();
                foreach (var label in deletedLabels)
                {
                    delTasks.Add(_taskLabelService.Delete(label.TaskLabelId));

                    CreateOrEditTaskHistoryDto historyDto = new CreateOrEditTaskHistoryDto { PaxTaskId = taskId, CreatedUser = currentUserId, FieldName = "Labels", NewValue = label.Name, ChangeType = EntityChangeType.Deleted, CreatedDate = DateTime.Now };
                    delTasks.Add(_taskHistoriesAppService.CreateOrEdit(historyDto));
                }

                Task.WaitAll(delTasks.ToArray());
            }

            var insertedLabels = updatedLabels.Where(x => existingLabels.Select(w => w.Id).Contains(x.Id) == false);

            if (insertedLabels != null && insertedLabels.Count() > 0)
            {
                List<Task> inserTasks = new List<Task>();
                int labelId = 0;
                foreach (var label in insertedLabels)
                {
                    if (!_labelRepository.GetAll().Any(x => x.Name.ToLower() == label.Name.ToLower()))
                    {
                        Label labelDto = new Label { Name = label.Name };

                        await _labelRepository.InsertAsync(labelDto);
                        await CurrentUnitOfWork.SaveChangesAsync();

                        labelId = labelDto.Id;
                    }
                    else
                    {
                        labelId = label.Id;
                    }

                    CreateOrEditTaskLabelDto taskLabelDto = new CreateOrEditTaskLabelDto { LabelId = labelId, PaxTaskId = taskId };
                    inserTasks.Add(_taskLabelService.CreateOrEdit(taskLabelDto));

                    CreateOrEditTaskHistoryDto historyDto = new CreateOrEditTaskHistoryDto { PaxTaskId = taskId, CreatedUser = currentUserId, FieldName = "Labels", NewValue = label.Name, ChangeType = EntityChangeType.Created, CreatedDate = DateTime.Now };
                    inserTasks.Add(_taskHistoriesAppService.CreateOrEdit(historyDto));
                }

                Task.WaitAll(inserTasks.ToArray());
            }
        }

        private async Task UpdateDependants(int taskId, List<TaskDependancyRelationDto> updatedDeps)
        {
            var existingDeps = _taskDependancyRelationsAppService.GetTasksDependecies(taskId).Result.ToList();

            var deletedDeps = existingDeps.Where(x => updatedDeps.Select(w => w.DependOnTaskId).Contains(x.DependOnTaskId) == false).ToList();

            var currentUserId = AbpSession.GetUserId();

            if (deletedDeps != null && deletedDeps.Count() > 0)
            {
                List<Task> delTasks = new List<Task>();
                foreach (var dep in deletedDeps)
                {
                    delTasks.Add(_taskDependancyRelationsAppService.Delete(dep.Id));

                    CreateOrEditTaskHistoryDto historyDto = new CreateOrEditTaskHistoryDto { PaxTaskId = taskId, CreatedUser = currentUserId, FieldName = "Dependencies", NewValue = dep.DependOnHeader, ChangeType = EntityChangeType.Deleted, CreatedDate = DateTime.Now };
                    delTasks.Add(_taskHistoriesAppService.CreateOrEdit(historyDto));
                }

                Task.WaitAll(delTasks.ToArray());
            }

            var insertedDeps = updatedDeps.Where(x => existingDeps.Select(w => w.DependOnTaskId).Contains(x.DependOnTaskId) == false);

            if (insertedDeps != null && insertedDeps.Count() > 0)
            {
                List<Task> inserTasks = new List<Task>();
                int labelId = 0;
                foreach (var dep in insertedDeps)
                {
                    CreateOrEditTaskDependancyRelationDto taskDepDto = new CreateOrEditTaskDependancyRelationDto { DependOnTaskId = dep.DependOnTaskId, PaxTaskId = taskId };
                    inserTasks.Add(_taskDependancyRelationsAppService.CreateOrEdit(taskDepDto));

                    CreateOrEditTaskHistoryDto historyDto = new CreateOrEditTaskHistoryDto { PaxTaskId = taskId, CreatedUser = currentUserId, FieldName = "Dependencies", NewValue = dep.DependOnHeader, ChangeType = EntityChangeType.Created, CreatedDate = DateTime.Now };
                    inserTasks.Add(_taskHistoriesAppService.CreateOrEdit(historyDto));
                }

                Task.WaitAll(inserTasks.ToArray());
            }
        }

        private async Task UpdateWatchers(int taskId, List<WatcherUserLookupTableDto> updatedWatchers)
        {
            var existingWatchers = _watcherRepository.GetByTaskId(taskId).Result.ToList();

            var deletedWatchers = existingWatchers.Where(x => updatedWatchers.Select(w => w.UserId).Contains(x.UserId) == false).ToList();

            var currentUserId = AbpSession.GetUserId();

            if (deletedWatchers != null && deletedWatchers.Count() > 0)
            {
                List<Task> delTasks = new List<Task>();
                foreach (var watcher in deletedWatchers)
                {
                    delTasks.Add(_watcherRepository.Delete(watcher.Id));

                    CreateOrEditTaskHistoryDto historyDto = new CreateOrEditTaskHistoryDto { PaxTaskId = taskId, CreatedUser = currentUserId, FieldName = "Watchers", NewValue = watcher.UserId.ToString(), ChangeType = EntityChangeType.Deleted, CreatedDate = DateTime.Now };
                    delTasks.Add(_taskHistoriesAppService.CreateOrEdit(historyDto));
                }

                Task.WaitAll(delTasks.ToArray());
            }

            var insertedWatchers = updatedWatchers.Where(x => existingWatchers.Select(w => w.UserId).Contains(x.UserId) == false);

            if (insertedWatchers != null && insertedWatchers.Count() > 0)
            {
                List<Task> inserTasks = new List<Task>();
                foreach (var watcher in insertedWatchers)
                {
                    CreateOrEditWatcherDto watcherDto = new CreateOrEditWatcherDto { UserId = watcher.UserId, PaxTaskId = taskId };
                    inserTasks.Add(_watcherRepository.CreateOrEdit(watcherDto));

                    CreateOrEditTaskHistoryDto historyDto = new CreateOrEditTaskHistoryDto { PaxTaskId = taskId, CreatedUser = currentUserId, FieldName = "Watchers", NewValue = watcher.UserId.ToString(), ChangeType = EntityChangeType.Created, CreatedDate = DateTime.Now };
                    inserTasks.Add(_taskHistoriesAppService.CreateOrEdit(historyDto));
                }

                Task.WaitAll(inserTasks.ToArray());
            }
        }

        [AbpAuthorize(AppPermissions.Pages_PaxTasks_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _paxTaskRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetPaxTasksToExcel(GetAllPaxTasksForExcelInput input)
        {
            var taskTypeFilter = input.TaskTypeFilter.HasValue
                        ? (TaskType)input.TaskTypeFilter
                        : default;
            var taskTypePeriodFilter = input.TaskTypePeriodFilter.HasValue
                ? (TaskTypePeriod)input.TaskTypePeriodFilter
                : default;

            var filteredPaxTasks = _paxTaskRepository.GetAll()
                        .Include(e => e.ReporterFk)
                        .Include(e => e.AssigneeFk)
                        .Include(e => e.SeverityFk)
                        .Include(e => e.TaskStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Header.Contains(input.Filter) || e.Details.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HeaderFilter), e => e.Header == input.HeaderFilter)
                        .WhereIf(input.MinCreatedDateFilter != null, e => e.CreatedDate >= input.MinCreatedDateFilter)
                        .WhereIf(input.MaxCreatedDateFilter != null, e => e.CreatedDate <= input.MaxCreatedDateFilter)
                        .WhereIf(input.TaskTypeFilter.HasValue && input.TaskTypeFilter > -1, e => e.TaskType == taskTypeFilter)
                        .WhereIf(input.TaskTypePeriodFilter.HasValue && input.TaskTypePeriodFilter > -1, e => e.TaskTypePeriod == taskTypePeriodFilter)
                        .WhereIf(input.MinPeriodIntervalFilter != null, e => e.PeriodInterval >= input.MinPeriodIntervalFilter)
                        .WhereIf(input.MaxPeriodIntervalFilter != null, e => e.PeriodInterval <= input.MaxPeriodIntervalFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.ReporterFk != null && e.ReporterFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.AssigneeFk != null && e.AssigneeFk.Name == input.UserName2Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SeverityNameFilter), e => e.SeverityFk != null && e.SeverityFk.Name == input.SeverityNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskStatusNameFilter), e => e.TaskStatusFk != null && e.TaskStatusFk.Name == input.TaskStatusNameFilter);

            var query = (from o in filteredPaxTasks
                         join o1 in _lookup_userRepository.GetAll() on o.ReporterId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_userRepository.GetAll() on o.AssigneeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_severityRepository.GetAll() on o.SeverityId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_taskStatusRepository.GetAll() on o.TaskStatusId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetPaxTaskForViewDto()
                         {
                             PaxTask = new PaxTaskDto
                             {
                                 Header = o.Header,
                                 CreatedDate = o.CreatedDate,
                                 TaskType = o.TaskType,
                                 TaskTypePeriod = o.TaskTypePeriod,
                                 PeriodInterval = o.PeriodInterval,
                                 Id = o.Id
                             },
                             UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             UserName2 = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             SeverityName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             TaskStatusName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });

            var paxTaskListDtos = await query.ToListAsync();

            return _paxTasksExcelExporter.ExportToFile(paxTaskListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_PaxTasks)]
        public async Task<PagedResultDto<PaxTaskUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll()
                .WhereIf(input.OmitIds != null, x => !input.OmitIds.Contains(x.Id))
                .WhereIf(
                       !string.IsNullOrWhiteSpace(input.Filter),
                      e => (e.Name != null && e.Name.Contains(input.Filter)) || (e.Surname != null && e.Surname.Contains(input.Filter))
                   ).Select(x => new { x.Id, x.FullName });

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<PaxTaskUserLookupTableDto>();

            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new PaxTaskUserLookupTableDto
                {
                    UserId = user.Id,
                    DisplayName = user.FullName?.ToString()
                });
            }

            return new PagedResultDto<PaxTaskUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_PaxTasks)]
        public async Task<List<LabelDto>> GetTaskLabels(GetAllLabelsInput input)
        {
            var query = _labelRepository.GetAll()
                .WhereIf(input.OmitIds != null, x => !input.OmitIds.Contains(x.Id))
                .WhereIf(
                       !string.IsNullOrWhiteSpace(input.Filter),
                      e => (e.Name != null && e.Name.Contains(input.Filter))
                   ).Select(x => new { x.Id, x.Name });

            var totalCount = await query.CountAsync();

            var LabelList = await query
                .Select(e => new LabelDto { Id = e.Id, Name = e.Name })
                .PageBy(input)
                .ToListAsync();

            LabelList.Insert(0, new LabelDto
            {
                Id = 1,
                Name = input.Filter
            });

            return LabelList;
        }

        [AbpAuthorize(AppPermissions.Pages_PaxTasks)]
        public async Task<PagedResultDto<GetUsersForMention>> GetUsersForMention(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll()
                .WhereIf(
                       !string.IsNullOrWhiteSpace(input.Filter),
                      e => (e.Name != null && e.Name.Contains(input.Filter)) || (e.Surname != null && e.Surname.Contains(input.Filter))
                   ).Select(x => new { x.Id, x.FullName, x.EmailAddress });

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<GetUsersForMention>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new GetUsersForMention
                {
                    Id = "@" + user.FullName?.ToString(),
                    UserId = user.Id,
                    Link = "mailto:" + user.EmailAddress,
                    DisplayName = user.FullName?.ToString()
                });
            }

            return new PagedResultDto<GetUsersForMention>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_PaxTasks)]
        public async Task<List<PaxTaskSeverityLookupTableDto>> GetAllSeverityForTableDropdown()
        {
            return await _lookup_severityRepository.GetAll()
                .Select(severity => new PaxTaskSeverityLookupTableDto
                {
                    Id = severity.Id,
                    DisplayName = severity == null || severity.Name == null ? "" : severity.Name.ToString(),
                    IconUrl = severity.IconUrl
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_PaxTasks)]
        public async Task<List<PaxTaskTaskStatusLookupTableDto>> GetAllTaskStatusForTableDropdown()
        {
            return await _lookup_taskStatusRepository.GetAll()
                .Select(taskStatus => new PaxTaskTaskStatusLookupTableDto
                {
                    Id = taskStatus.Id,
                    DisplayName = taskStatus == null || taskStatus.Name == null ? "" : taskStatus.Name.ToString(),
                    IconUrl = taskStatus.IconUrl
                }).ToListAsync();
        }


        #region Audit

        [AbpAuthorize(AppPermissions.Pages_PaxTasks)]
        public async Task<PagedResultDto<HistoryDto>> GetTaskHistory(int taskId)
        {
            List<HistoryDto> histories = new List<HistoryDto>();

            GetAllTaskHistoriesInput input = new GetAllTaskHistoriesInput { PaxTaskIdFilter = taskId };
            var results = _taskHistoriesAppService.GetAll(input).Result.Items;

            var watchersIds = results.Where(x => x.TaskHistory.FieldName == "Watchers").Select(x => long.Parse(x.TaskHistory.NewValue)).ToList();

            var watchers = _lookup_userRepository.GetAll().Where(x => watchersIds.Contains(x.Id)).Select(x => new NameValueDto { Name = x.Id.ToString(), Value = x.FullName }).ToList();

            var attachments = _paxTaskAttachmentRepository.GetAll().Where(x => x.PaxTaskId == taskId).Select(x => new PaxTaskAttachmentDto { Id = x.Id, FileName = x.FileName }).ToList();

            foreach (var result in results)
            {

                HistoryDto history = new HistoryDto { Id = result.TaskHistory.Id, CreationTime = result.TaskHistory.CreatedTime, ChangeText = GetChangeText(result.TaskHistory.ChangeType, result.UserName, result.TaskHistory.OldValue, result.TaskHistory.NewValue, result.TaskHistory.FieldName, null, null, watchers, attachments) };
                histories.Add(history);
            }

            histories.AddRange(CreateEntityChangesAndUsers(taskId));

            histories = histories.OrderByDescending(x => x.CreationTime).ToList();

            var resultCount = histories.Count;

            return new PagedResultDto<HistoryDto>(resultCount, histories);
        }

        private List<HistoryDto> CreateEntityChangesAndUsers(int taskId)
        {
            List<HistoryDto> historyRecs = new List<HistoryDto>();

            List<string> ids = new List<string>();

            ids.Add(taskId.ToString());

            List<string> trackedFieldNames = new List<string> { "UserId", "TaskType", "SeverityId", "TaskStatusId", "Header", "Details", "TaskTypePeriod", "PeriodInterval", "DeadLineDate" };

            string text = _localizationSource.GetString("PaxTaskHistoryText");

            //List
            var histories = (from entityChangeSet in _entityChangeSetRepository.GetAll()
                             join entityChange in _entityChangeRepository.GetAll() on entityChangeSet.Id equals entityChange.EntityChangeSetId
                             join propChange in _entityPropertyChangeRepository.GetAll() on entityChange.Id equals propChange.EntityChangeId
                             join user in _lookup_userRepository.GetAll() on entityChangeSet.UserId equals user.Id
                             where entityChange.EntityId == taskId.ToString() && trackedFieldNames.Contains(propChange.PropertyName)
                             select new
                             {
                                 EntityTypeFullName = entityChange.EntityTypeFullName,
                                 ChangeType = entityChange.ChangeType,
                                 FullName = user.FullName,
                                 OriginalValue = propChange.OriginalValue,
                                 NewValue = propChange.NewValue,
                                 ChangeTime = entityChange.ChangeTime,
                                 FieldName = propChange.PropertyName
                             }).ToList();

            if (histories != null)
            {
                var severities = _lookup_severityRepository.GetAll().ToList();
                var taskStatuses = _lookup_taskStatusRepository.GetAll().ToList();

                foreach (var item in histories)
                {
                    historyRecs.Add(new HistoryDto { CreationTime = item.ChangeTime, ChangeText = GetChangeText(item.ChangeType, item.FullName, item.OriginalValue, item.NewValue, item.FieldName, severities, taskStatuses) });
                }
            }

            return historyRecs;
        }

        string GetChangeText(EntityChangeType changeType, string userFullName, string oldValue, string newValue, string fieldName, List<Severity> severities = null, List<TaskStatus> taskStatuses = null, List<NameValueDto> watchers = null, List<PaxTaskAttachmentDto> attchs = null)
        {
            switch (fieldName)
            {
                case "Labels":

                    if (changeType == EntityChangeType.Created)
                    {
                        return _localizationSource.GetString("PaxTaskHistoryLabelAdd").Replace("{0}", "<b>" + userFullName + "</b>").Replace("{2}", "<b>" + newValue + "</b>");
                    }
                    else if (changeType == EntityChangeType.Deleted)
                    {
                        return _localizationSource.GetString("PaxTaskHistoryLabelDel").Replace("{0}", "<b>" + userFullName + "</b>").Replace("{2}", "<b>" + newValue + "</b>");
                    }
                    break;
                case "Watchers":

                    string targetUser = watchers.Where(x => x.Name == newValue).Select(x => x.Value).FirstOrDefault();

                    if (changeType == EntityChangeType.Created)
                    {
                        return _localizationSource.GetString("PaxTaskHistoryWatcherAdd").Replace("{0}", "<b>" + userFullName + "</b>").Replace("{2}", "<b>" + targetUser + "</b>");
                    }
                    else if (changeType == EntityChangeType.Deleted)
                    {
                        return _localizationSource.GetString("PaxTaskHistoryWatcherDel").Replace("{0}", "<b>" + userFullName + "</b>").Replace("{2}", "<b>" + targetUser + "</b>");
                    }
                    break;
                case "Dependencies":

                    if (changeType == EntityChangeType.Created)
                    {
                        return _localizationSource.GetString("PaxTaskHistoryDependencyAdd").Replace("{0}", "<b>" + userFullName + "</b>").Replace("{2}", "<b>" + newValue + "</b>");
                    }
                    else if (changeType == EntityChangeType.Deleted)
                    {
                        return _localizationSource.GetString("PaxTaskHistoryDependencyDel").Replace("{0}", "<b>" + userFullName + "</b>").Replace("{2}", "<b>" + newValue + "</b>");
                    }
                    break;
                case "Comments":
                    if (changeType == EntityChangeType.Created)
                    {
                        return _localizationSource.GetString("PaxTaskHistoryCommentAdd").Replace("{0}", "<b>" + userFullName + "</b>");
                    }
                    else if (changeType == EntityChangeType.Updated)
                    {
                        return _localizationSource.GetString("PaxTaskHistoryCommentUpt").Replace("{0}", "<b>" + userFullName + "</b>");
                    }
                    else if (changeType == EntityChangeType.Deleted)
                    {
                        return _localizationSource.GetString("PaxTaskHistoryCommentDel").Replace("{0}", "<b>" + userFullName + "</b>");
                    }
                    break;
                case "Attachments":
                    string fileName = attchs.Where(x => x.Id == int.Parse(newValue)).Select(x => x.FileName).FirstOrDefault();
                    if (changeType == EntityChangeType.Created)
                    {
                        return _localizationSource.GetString("PaxTaskHistoryAttchAdd").Replace("{0}", "<b>" + userFullName + "</b>").Replace("{2}", "<b>" + fileName + "</b>");
                    }
                    else if (changeType == EntityChangeType.Deleted)
                    {
                        return _localizationSource.GetString("PaxTaskHistoryAttchDel").Replace("{0}", "<b>" + userFullName + "</b>").Replace("{2}", "<b>" + fileName + "</b>");
                    }
                    break;
                case "DeadLineDate":
                    return _localizationSource.GetString("PaxTaskHistoryText").Replace("{0}", "<b>" + userFullName + "</b>").Replace("{1}", "<b>" + _localizationSource.GetString(fieldName) + "</b>").Replace("{2}", "<b>" + DateTime.Parse(newValue.Replace("\"", "")).ToShortDateString() + "</b>");
                case "SeverityId":
                    return _localizationSource.GetString("PaxTaskHistoryText").Replace("{0}", "<b>" + userFullName + "</b>").Replace("{1}", "<b>" + _localizationSource.GetString("SeverityName") + "</b>").Replace("{2}", "<b>" + severities.Where(x => x.Id.ToString() == newValue).Select(x => x.Name).FirstOrDefault() + "</b>");
                case "TaskStatusId":
                    return _localizationSource.GetString("PaxTaskHistoryText").Replace("{0}", "<b>" + userFullName + "</b>").Replace("{1}", "<b>" + _localizationSource.GetString("TaskStatusName") + "</b>").Replace("{2}", "<b>" + taskStatuses.Where(x => x.Id.ToString() == newValue).Select(x => x.Name).FirstOrDefault() + "</b>");
                case "TaskType":

                    var val = string.Empty;

                    switch (newValue)
                    {
                        case "1":
                            val = "Normal";
                            break;
                        case "2":
                            val = _localizationSource.GetString("Repeating");
                            break;
                        case "3":
                            val = _localizationSource.GetString("DeadLine");
                            break;
                    }

                    return _localizationSource.GetString("PaxTaskHistoryText").Replace("{0}", "<b>" + userFullName + "</b>").Replace("{1}", "<b>" + _localizationSource.GetString(fieldName) + "</b>").Replace("{2}", "<b>" + val + "</b>");

                case "TaskTypePeriod":
                    return _localizationSource.GetString("PaxTaskHistoryText").Replace("{0}", "<b>" + userFullName + "</b>").Replace("{1}", "<b>" + _localizationSource.GetString(fieldName) + "</b>").Replace("{2}", "<b>" + newValue == "1" ? "<b>" + _localizationSource.GetString("Weekly") + "</b>" : "<b>" + _localizationSource.GetString("Monthly") + "</b>");
                default:
                    return _localizationSource.GetString("PaxTaskHistoryText").Replace("{0}", "<b>" + userFullName + "</b>").Replace("{1}", "<b>" + _localizationSource.GetString(fieldName) + "</b>").Replace("{2}", "<b>" + newValue + "</b>");

            }

            return "";
        }

        #endregion

    }
}