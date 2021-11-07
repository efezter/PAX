using System.Threading.Tasks;
using Abp.Application.Services;
using PAX.Next.MultiTenancy.Payments.PayPal.Dto;

namespace PAX.Next.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
