using System.Collections.Generic;
using Abp.Application.Services.Dto;
using PAX.TaskManager.Authorization.Permissions.Dto;
using PAX.TaskManager.Web.Areas.Admin.Models.Common;

namespace PAX.TaskManager.Web.Areas.Admin.Models.Roles
{
    public class RoleListViewModel : IPermissionsEditViewModel
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}