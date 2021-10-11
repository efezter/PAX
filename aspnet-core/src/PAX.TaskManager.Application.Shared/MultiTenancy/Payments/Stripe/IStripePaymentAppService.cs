using System.Threading.Tasks;
using Abp.Application.Services;
using PAX.TaskManager.MultiTenancy.Payments.Dto;
using PAX.TaskManager.MultiTenancy.Payments.Stripe.Dto;

namespace PAX.TaskManager.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}