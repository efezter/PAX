using System.Threading.Tasks;
using Abp.Application.Services;

namespace PAX.Next.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
