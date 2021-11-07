using System.Linq;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using PAX.Next.Authorization.Users.Dto;
using PAX.Next.Security;
using PAX.Next.Web.Areas.Admin.Models.Common;

namespace PAX.Next.Web.Areas.Admin.Models.Users
{
    [AutoMapFrom(typeof(GetUserForEditOutput))]
    public class CreateOrEditUserModalViewModel : GetUserForEditOutput, IOrganizationUnitsEditViewModel
    {
        public bool CanChangeUserName => User.UserName != AbpUserBase.AdminUserName;

        public int AssignedRoleCount
        {
            get { return Roles.Count(r => r.IsAssigned); }
        }

        public bool IsEditMode => User.Id.HasValue;

        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }
    }
}