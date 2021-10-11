using PAX.TaskManager.Editions.Dto;

namespace PAX.TaskManager.MultiTenancy.Payments.Dto
{
    public class PaymentInfoDto
    {
        public EditionSelectDto Edition { get; set; }

        public decimal AdditionalPrice { get; set; }

        public bool IsLessThanMinimumUpgradePaymentAmount()
        {
            return AdditionalPrice < TaskManagerConsts.MinimumUpgradePaymentAmount;
        }
    }
}
