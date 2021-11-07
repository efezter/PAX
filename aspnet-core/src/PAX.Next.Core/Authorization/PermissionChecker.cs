using Abp.Authorization;
using PAX.Next.Authorization.Roles;
using PAX.Next.Authorization.Users;

namespace PAX.Next.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
