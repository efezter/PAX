using System.Collections.Generic;
using PAX.TaskManager.Editions.Dto;
using PAX.TaskManager.MultiTenancy.Payments;

namespace PAX.TaskManager.Web.Models.Payment
{
    public class ExtendEditionViewModel
    {
        public EditionSelectDto Edition { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}