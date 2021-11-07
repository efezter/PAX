using System.Collections.Generic;
using PAX.Next.Editions.Dto;
using PAX.Next.MultiTenancy.Payments;

namespace PAX.Next.Web.Models.Payment
{
    public class ExtendEditionViewModel
    {
        public EditionSelectDto Edition { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}