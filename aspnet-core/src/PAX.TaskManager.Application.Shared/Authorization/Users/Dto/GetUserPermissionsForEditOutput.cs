using System.Collections.Generic;
using PAX.TaskManager.Authorization.Permissions.Dto;

namespace PAX.TaskManager.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}