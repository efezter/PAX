using Abp.Application.Services;
using Abp.Application.Services.Dto;
using PAX.TaskManager.Authorization.Permissions.Dto;

namespace PAX.TaskManager.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
