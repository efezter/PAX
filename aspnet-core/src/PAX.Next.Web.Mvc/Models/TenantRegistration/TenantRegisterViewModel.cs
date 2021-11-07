using PAX.Next.Editions;
using PAX.Next.Editions.Dto;
using PAX.Next.MultiTenancy.Payments;
using PAX.Next.Security;
using PAX.Next.MultiTenancy.Payments.Dto;

namespace PAX.Next.Web.Models.TenantRegistration
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
