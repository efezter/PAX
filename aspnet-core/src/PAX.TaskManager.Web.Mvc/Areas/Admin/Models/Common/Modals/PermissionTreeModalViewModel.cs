using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PAX.TaskManager.Authorization.Permissions.Dto;

namespace PAX.TaskManager.Web.Areas.Admin.Models.Common.Modals
{
    public class PermissionTreeModalViewModel : IPermissionsEditViewModel
    {
        public List<FlatPermissionDto> Permissions { get; set; }
        public List<string> GrantedPermissionNames { get; set; }
    }
}
