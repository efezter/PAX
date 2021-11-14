﻿using PAX.Next.Authorization.Users;
using PAX.Next.Authorization.Users;
using PAX.Next.TaskManager;

using PAX.Next.TaskManager.Utils;

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
using static PAX.Next.TaskManager.Utils.Enums;
using Abp.Runtime.Session;

namespace PAX.Next.TaskManager
{
    [AbpAuthorize(AppPermissions.Pages_PaxTasks)]
    public class PaxTasksAppService : NextAppServiceBase, IPaxTasksAppService
    {
        private readonly IRepository<PaxTask> _paxTaskRepository;
        private readonly IPaxTasksExcelExporter _paxTasksExcelExporter;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<Severity, int> _lookup_severityRepository;
        private readonly IRepository<TaskStatus, int> _lookup_taskStatusRepository;
        private readonly IAbpSession _abpSession;

        public PaxTasksAppService(IRepository<PaxTask> paxTaskRepository, IPaxTasksExcelExporter paxTasksExcelExporter, IRepository<User, long> lookup_userRepository, IRepository<Severity, int> lookup_severityRepository, IRepository<TaskStatus, int> lookup_taskStatusRepository, IAbpSession abpSession)
        {
            _paxTaskRepository = paxTaskRepository;
            _paxTasksExcelExporter = paxTasksExcelExporter;
            _lookup_userRepository = lookup_userRepository;
            _lookup_severityRepository = lookup_severityRepository;
            _lookup_taskStatusRepository = lookup_taskStatusRepository;
            _abpSession = abpSession;

        }

        public async Task<PagedResultDto<GetPaxTaskForViewDto>> GetAll(GetAllPaxTasksInput input)
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

            var pagedAndFilteredPaxTasks = filteredPaxTasks
                .OrderBy(input.Sorting ?? "id asc")
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
                               Id = o.Id,
                               UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                               UserName2 = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                               SeverityName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
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
                        Id = o.Id,
                    },
                    UserName = o.UserName,
                    UserName2 = o.UserName2,
                    SeverityName = o.SeverityName,
                    TaskStatusName = o.TaskStatusName
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

            if (output.PaxTask.ReporterId != null)
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

            if (output.PaxTask.TaskStatusId != null)
            {
                var _lookupTaskStatus = await _lookup_taskStatusRepository.FirstOrDefaultAsync((int)output.PaxTask.TaskStatusId);
                output.TaskStatusName = _lookupTaskStatus?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPaxTaskDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PaxTasks_Create)]
        protected virtual async Task Create(CreateOrEditPaxTaskDto input)
        {
            var paxTask = ObjectMapper.Map<PaxTask>(input);

            paxTask.CreatedDate = DateTime.Now;
            paxTask.ReporterId = _abpSession.GetUserId();

            await _paxTaskRepository.InsertAsync(paxTask);

        }

        [AbpAuthorize(AppPermissions.Pages_PaxTasks_Edit)]
        protected virtual async Task Update(CreateOrEditPaxTaskDto input)
        {
            var paxTask = await _paxTaskRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, paxTask);

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
            try
            {
                var query = _lookup_userRepository.GetAll().WhereIf(
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
                        Id = user.Id,
                        DisplayName = user.FullName?.ToString()
                    });
                }

                return new PagedResultDto<PaxTaskUserLookupTableDto>(
                    totalCount,
                    lookupTableDtoList
                );
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [AbpAuthorize(AppPermissions.Pages_PaxTasks)]
        public async Task<List<PaxTaskSeverityLookupTableDto>> GetAllSeverityForTableDropdown()
        {
            return await _lookup_severityRepository.GetAll()
                .Select(severity => new PaxTaskSeverityLookupTableDto
                {
                    Id = severity.Id,
                    DisplayName = severity == null || severity.Name == null ? "" : severity.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_PaxTasks)]
        public async Task<List<PaxTaskTaskStatusLookupTableDto>> GetAllTaskStatusForTableDropdown()
        {
            return await _lookup_taskStatusRepository.GetAll()
                .Select(taskStatus => new PaxTaskTaskStatusLookupTableDto
                {
                    Id = taskStatus.Id,
                    DisplayName = taskStatus == null || taskStatus.Name == null ? "" : taskStatus.Name.ToString()
                }).ToListAsync();
        }

    }
}