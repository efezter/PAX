using System.Collections.Generic;
using PAX.TaskManager.Authorization.Permissions.Dto;

namespace PAX.TaskManager.Web.Areas.Admin.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}