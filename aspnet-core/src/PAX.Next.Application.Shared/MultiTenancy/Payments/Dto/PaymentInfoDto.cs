using PAX.Next.Editions.Dto;

namespace PAX.Next.MultiTenancy.Payments.Dto
{
    public class PaymentInfoDto
    {
        public EditionSelectDto Edition { get; set; }

        public decimal AdditionalPrice { get; set; }

        public bool IsLessThanMinimumUpgradePaymentAmount()
        {
            return AdditionalPrice < NextConsts.MinimumUpgradePaymentAmount;
        }
    }
}
