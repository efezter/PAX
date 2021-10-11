using System.Threading.Tasks;
using Abp.Application.Services;
using PAX.TaskManager.MultiTenancy.Payments.PayPal.Dto;

namespace PAX.TaskManager.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
