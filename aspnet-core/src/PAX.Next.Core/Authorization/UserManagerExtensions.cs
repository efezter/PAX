using System.Threading.Tasks;
using Abp.Authorization.Users;
using PAX.Next.Authorization.Users;

namespace PAX.Next.Authorization
{
    public static class UserManagerExtensions
    {
        public static async Task<User> GetAdminAsync(this UserManager userManager)
        {
            return await userManager.FindByNameAsync(AbpUserBase.AdminUserName);
        }
    }
}
