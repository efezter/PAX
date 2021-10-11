using Abp.Authorization;
using PAX.TaskManager.Authorization.Roles;
using PAX.TaskManager.Authorization.Users;

namespace PAX.TaskManager.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
