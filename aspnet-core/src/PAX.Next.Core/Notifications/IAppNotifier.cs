using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Abp.Localization;
using Abp.Notifications;
using PAX.Next.Authorization.Users;
using PAX.Next.MultiTenancy;

namespace PAX.Next.Notifications
{
    public interface IAppNotifier
    {
        Task WelcomeToTheApplicationAsync(User user);

        Task NewUserRegisteredAsync(User user);

        Task NewTenantRegisteredAsync(Tenant tenant);

        Task GdprDataPrepared(UserIdentifier user, Guid binaryObjectId);

        Task SendMessageAsync(UserIdentifier user, string message, NotificationSeverity severity = NotificationSeverity.Info);

        Task SendMessageAsync(UserIdentifier user, LocalizableString localizableMessage, IDictionary<string, object> localizableMessageData = null, NotificationSeverity severity = NotificationSeverity.Info);

        Task TenantsMovedToEdition(UserIdentifier user, string sourceEditionName, string targetEditionName);

        Task SomeUsersCouldntBeImported(UserIdentifier user, string fileToken, string fileType, string fileName);

        Task TaskChangedAsync(string reporterName, int taskId, string messageId, UserIdentifier[] user);

        //Task TaskRegisteredAsync(string reporterName, int taskId, UserIdentifier[] receivers);
    }
}
