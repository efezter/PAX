using System.Collections.Generic;
using PAX.TaskManager.Editions;
using PAX.TaskManager.Editions.Dto;
using PAX.TaskManager.MultiTenancy.Payments;
using PAX.TaskManager.MultiTenancy.Payments.Dto;

namespace PAX.TaskManager.Web.Models.Payment
{
    public class BuyEditionViewModel
    {
        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public decimal? AdditionalPrice { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}
