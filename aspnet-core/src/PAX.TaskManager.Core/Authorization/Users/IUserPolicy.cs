using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace PAX.TaskManager.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
