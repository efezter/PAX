using System.Collections.Generic;
using PAX.Next.Editions;
using PAX.Next.Editions.Dto;
using PAX.Next.MultiTenancy.Payments;
using PAX.Next.MultiTenancy.Payments.Dto;

namespace PAX.Next.Web.Models.Payment
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
