using Abp.Application.Services;
using Abp.Application.Services.Dto;
using PAX.Next.Authorization.Permissions.Dto;

namespace PAX.Next.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
