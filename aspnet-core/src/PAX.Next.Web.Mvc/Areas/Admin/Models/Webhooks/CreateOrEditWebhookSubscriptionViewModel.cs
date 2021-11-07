using Abp.Application.Services.Dto;
using Abp.Webhooks;
using PAX.Next.WebHooks.Dto;

namespace PAX.Next.Web.Areas.Admin.Models.Webhooks
{
    public class CreateOrEditWebhookSubscriptionViewModel
    {
        public WebhookSubscription WebhookSubscription { get; set; }

        public ListResultDto<GetAllAvailableWebhooksOutput> AvailableWebhookEvents { get; set; }
    }
}
