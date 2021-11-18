using PAX.Next.TaskManager;
using PAX.Next.Authorization.Users;

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
    [AbpAuthorize(AppPermissions.Pages_Watchers)]
    public class WatchersAppService : NextAppServiceBase, IWatchersAppService
    {
        private readonly IRepository<Watcher> _watcherRepository;
        private readonly IRepository<PaxTask, int> _lookup_paxTaskRepository;
        private readonly IRepository<User, long> _lookup_userRepository;

        public WatchersAppService(IRepository<Watcher> watcherRepository, IRepository<PaxTask, int> lookup_paxTaskRepository, IRepository<User, long> lookup_userRepository)
        {
            _watcherRepository = watcherRepository;
            _lookup_paxTaskRepository = lookup_paxTaskRepository;
            _lookup_userRepository = lookup_userRepository;

        }

        public async Task<IEnumerable<int>> GetByTaskId(int taskId)
        {

            var filteredWatchers = _watcherRepository.GetAll()
                        .Where(x => x.PaxTaskId == taskId)
                        .Select(x => x.Id);


            return filteredWatchers;
        }


        public async Task<IEnumerable<WatcherUserLookupTableDto>> GetUserDetailsByTaskId(int taskId)
        {

            var filteredWatchers = _watcherRepository.GetAll()
                        .Where(x => x.PaxTaskId == taskId)
                        .Select(x => x.UserId)
                        .ToList();
            

            return await GetAllUserForLookupTable(filteredWatchers);
        }


            public async Task<PagedResultDto<GetWatcherForViewDto>> GetAll(GetAllWatchersInput input)
        {

            var filteredWatchers = _watcherRepository.GetAll()
                        .Include(e => e.PaxTaskFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaxTaskHeaderFilter), e => e.PaxTaskFk != null && e.PaxTaskFk.Header == input.PaxTaskHeaderFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var pagedAndFilteredWatchers = filteredWatchers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var watchers = from o in pagedAndFilteredWatchers
                           join o1 in _lookup_paxTaskRepository.GetAll() on o.PaxTaskId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                           from s2 in j2.DefaultIfEmpty()

                           select new
                           {

                               Id = o.Id,
                               PaxTaskHeader = s1 == null || s1.Header == null ? "" : s1.Header.ToString(),
                               UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                           };

            var totalCount = await filteredWatchers.CountAsync();

            var dbList = await watchers.ToListAsync();
            var results = new List<GetWatcherForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetWatcherForViewDto()
                {
                    Watcher = new WatcherDto
                    {

                        Id = o.Id,
                    },
                    PaxTaskHeader = o.PaxTaskHeader,
                    UserName = o.UserName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetWatcherForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_Watchers_Edit)]
        public async Task<GetWatcherForEditOutput> GetWatcherForEdit(EntityDto input)
        {
            var watcher = await _watcherRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetWatcherForEditOutput { Watcher = ObjectMapper.Map<CreateOrEditWatcherDto>(watcher) };

            if (output.Watcher.PaxTaskId != null)
            {
                var _lookupPaxTask = await _lookup_paxTaskRepository.FirstOrDefaultAsync((int)output.Watcher.PaxTaskId);
                output.PaxTaskHeader = _lookupPaxTask?.Header?.ToString();
            }

            if (output.Watcher.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Watcher.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditWatcherDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Watchers_Create)]
        protected virtual async Task Create(CreateOrEditWatcherDto input)
        {
            var watcher = ObjectMapper.Map<Watcher>(input);

            await _watcherRepository.InsertAsync(watcher);

        }

        [AbpAuthorize(AppPermissions.Pages_Watchers_Edit)]
        protected virtual async Task Update(CreateOrEditWatcherDto input)
        {
            var watcher = await _watcherRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, watcher);

        }

        [AbpAuthorize(AppPermissions.Pages_Watchers_Delete)]
        public async Task Delete(long id)
        {
            await _watcherRepository.DeleteAsync(int.Parse(id.ToString()));
        }

        [AbpAuthorize(AppPermissions.Pages_Watchers)]
        public async Task<PagedResultDto<WatcherPaxTaskLookupTableDto>> GetAllPaxTaskForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_paxTaskRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Header != null && e.Header.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var paxTaskList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<WatcherPaxTaskLookupTableDto>();
            foreach (var paxTask in paxTaskList)
            {
                lookupTableDtoList.Add(new WatcherPaxTaskLookupTableDto
                {
                    Id = paxTask.Id,
                    DisplayName = paxTask.Header?.ToString()
                });
            }

            return new PagedResultDto<WatcherPaxTaskLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Watchers)]
        public async Task<IEnumerable<WatcherUserLookupTableDto>> GetAllUserForLookupTable(List<long> watcherIds)
        {
            var query = _lookup_userRepository.GetAll()
                .Where(e => watcherIds.Contains(e.Id));

            var totalCount = await query.CountAsync();

            var userList = await query
                .ToListAsync();

            var lookupTableDtoList = new List<WatcherUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new WatcherUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.FullName?.ToString()
                });
            }

            return lookupTableDtoList;
        }

    }
}