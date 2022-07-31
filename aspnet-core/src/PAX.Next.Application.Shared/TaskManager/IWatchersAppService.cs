using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using PAX.Next.TaskManager.Dtos;
using PAX.Next.Dto;
using System.Collections.Generic;
using System.Linq;

namespace PAX.Next.TaskManager
{
    public interface IWatchersAppService : IApplicationService
    {
        Task<IEnumerable<WatcherUserLookupTableDto>> GetByTaskId(int taskId);

        Task<IEnumerable<WatcherUserLookupTableDto>> GetUserDetailsByTaskId(int taskId);

        Task<PagedResultDto<GetWatcherForViewDto>> GetAll(GetAllWatchersInput input);

        Task<GetWatcherForEditOutput> GetWatcherForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditWatcherDto input);

        Task Delete(long id);

        Task<PagedResultDto<WatcherPaxTaskLookupTableDto>> GetAllPaxTaskForLookupTable(GetAllForLookupTableInput input);

    }
}