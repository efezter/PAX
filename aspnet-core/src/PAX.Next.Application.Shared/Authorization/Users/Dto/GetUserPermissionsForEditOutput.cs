using System.Collections.Generic;
using PAX.Next.Authorization.Permissions.Dto;

namespace PAX.Next.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}