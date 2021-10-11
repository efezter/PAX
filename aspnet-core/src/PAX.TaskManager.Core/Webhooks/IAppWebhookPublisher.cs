using System.Threading.Tasks;
using PAX.TaskManager.Authorization.Users;

namespace PAX.TaskManager.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
