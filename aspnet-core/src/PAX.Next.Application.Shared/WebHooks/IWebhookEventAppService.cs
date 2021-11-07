using System.Threading.Tasks;
using Abp.Webhooks;

namespace PAX.Next.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
