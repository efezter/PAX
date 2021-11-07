using System.Collections.Generic;
using Abp.Application.Services.Dto;
using PAX.Next.Authorization.Permissions.Dto;
using PAX.Next.Web.Areas.Admin.Models.Common;

namespace PAX.Next.Web.Areas.Admin.Models.Users
{
    public class UsersViewModel : IPermissionsEditViewModel
    {
        public string FilterText { get; set; }

        public List<ComboboxItemDto> Roles { get; set; }

        public bool OnlyLockedUsers { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}
