using PAX.TaskManager.Editions;
using PAX.TaskManager.Editions.Dto;
using PAX.TaskManager.MultiTenancy.Payments;
using PAX.TaskManager.Security;
using PAX.TaskManager.MultiTenancy.Payments.Dto;

namespace PAX.TaskManager.Web.Models.TenantRegistration
{
    public class TenantRegisterViewModel
    {
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public int? EditionId { get; set; }

        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }
    }
}
