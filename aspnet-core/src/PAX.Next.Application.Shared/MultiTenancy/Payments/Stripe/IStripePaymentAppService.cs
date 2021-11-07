using System.Threading.Tasks;
using Abp.Application.Services;
using PAX.Next.MultiTenancy.Payments.Dto;
using PAX.Next.MultiTenancy.Payments.Stripe.Dto;

namespace PAX.Next.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}