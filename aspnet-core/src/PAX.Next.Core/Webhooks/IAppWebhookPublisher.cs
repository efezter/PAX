using System.Threading.Tasks;
using PAX.Next.Authorization.Users;

namespace PAX.Next.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
