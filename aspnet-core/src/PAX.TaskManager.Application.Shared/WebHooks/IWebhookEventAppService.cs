using System.Threading.Tasks;
using Abp.Webhooks;

namespace PAX.TaskManager.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
