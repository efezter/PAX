using Abp.Application.Services.Dto;
using Abp.Webhooks;
using PAX.TaskManager.WebHooks.Dto;

namespace PAX.TaskManager.Web.Areas.Admin.Models.Webhooks
{
    public class CreateOrEditWebhookSubscriptionViewModel
    {
        public WebhookSubscription WebhookSubscription { get; set; }

        public ListResultDto<GetAllAvailableWebhooksOutput> AvailableWebhookEvents { get; set; }
    }
}
