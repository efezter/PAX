using System.Collections.Generic;
using PAX.Next.Authorization.Permissions.Dto;

namespace PAX.Next.Web.Areas.Admin.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}