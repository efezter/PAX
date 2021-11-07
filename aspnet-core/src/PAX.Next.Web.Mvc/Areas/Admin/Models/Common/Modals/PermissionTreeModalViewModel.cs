using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PAX.Next.Authorization.Permissions.Dto;

namespace PAX.Next.Web.Areas.Admin.Models.Common.Modals
{
    public class PermissionTreeModalViewModel : IPermissionsEditViewModel
    {
        public List<FlatPermissionDto> Permissions { get; set; }
        public List<string> GrantedPermissionNames { get; set; }
    }
}
