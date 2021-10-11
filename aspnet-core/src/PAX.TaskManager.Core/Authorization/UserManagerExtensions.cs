using System.Threading.Tasks;
using Abp.Authorization.Users;
using PAX.TaskManager.Authorization.Users;

namespace PAX.TaskManager.Authorization
{
    public static class UserManagerExtensions
    {
        public static async Task<User> GetAdminAsync(this UserManager userManager)
        {
            return await userManager.FindByNameAsync(AbpUserBase.AdminUserName);
        }
    }
}
