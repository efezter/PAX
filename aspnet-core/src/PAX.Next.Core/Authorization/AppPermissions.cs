namespace PAX.Next.Authorization
{
    /// <summary>
    /// Defines string constants for application's permission names.
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public static class AppPermissions
    {
        public const string Pages_TaskDependancyRelations = "Pages.TaskDependancyRelations";
        public const string Pages_TaskDependancyRelations_Create = "Pages.TaskDependancyRelations.Create";
        public const string Pages_TaskDependancyRelations_Edit = "Pages.TaskDependancyRelations.Edit";
        public const string Pages_TaskDependancyRelations_Delete = "Pages.TaskDependancyRelations.Delete";

        public const string Pages_TaskLabels = "Pages.TaskLabels";
        public const string Pages_TaskLabels_Create = "Pages.TaskLabels.Create";
        public const string Pages_TaskLabels_Edit = "Pages.TaskLabels.Edit";
        public const string Pages_TaskLabels_Delete = "Pages.TaskLabels.Delete";

        public const string Pages_Labels = "Pages.Labels";
        public const string Pages_Labels_Create = "Pages.Labels.Create";
        public const string Pages_Labels_Edit = "Pages.Labels.Edit";
        public const string Pages_Labels_Delete = "Pages.Labels.Delete";

        public const string Pages_TaskHistories = "Pages.TaskHistories";
        public const string Pages_TaskHistories_Create = "Pages.TaskHistories.Create";
        public const string Pages_TaskHistories_Edit = "Pages.TaskHistories.Edit";
        public const string Pages_TaskHistories_Delete = "Pages.TaskHistories.Delete";

        public const string Pages_PaxTaskAttachments = "Pages.PaxTaskAttachments";
        public const string Pages_PaxTaskAttachments_Create = "Pages.PaxTaskAttachments.Create";
        public const string Pages_PaxTaskAttachments_Edit = "Pages.PaxTaskAttachments.Edit";
        public const string Pages_PaxTaskAttachments_Delete = "Pages.PaxTaskAttachments.Delete";

        public const string Pages_Comments = "Pages.Comments";
        public const string Pages_Comments_Create = "Pages.Comments.Create";
        public const string Pages_Comments_Edit = "Pages.Comments.Edit";
        public const string Pages_Comments_Delete = "Pages.Comments.Delete";

        public const string Pages_Watchers = "Pages.Watchers";
        public const string Pages_Watchers_Create = "Pages.Watchers.Create";
        public const string Pages_Watchers_Edit = "Pages.Watchers.Edit";
        public const string Pages_Watchers_Delete = "Pages.Watchers.Delete";

        public const string Pages_PaxTasks = "Pages.PaxTasks";
        public const string Pages_PaxTasks_Create = "Pages.PaxTasks.Create";
        public const string Pages_PaxTasks_Edit = "Pages.PaxTasks.Edit";
        public const string Pages_PaxTasks_Delete = "Pages.PaxTasks.Delete";

        public const string Pages_Tags = "Pages.Tags";
        public const string Pages_Tags_Create = "Pages.Tags.Create";
        public const string Pages_Tags_Edit = "Pages.Tags.Edit";
        public const string Pages_Tags_Delete = "Pages.Tags.Delete";

        public const string Pages_TaskStatuses = "Pages.TaskStatuses";
        public const string Pages_TaskStatuses_Create = "Pages.TaskStatuses.Create";
        public const string Pages_TaskStatuses_Edit = "Pages.TaskStatuses.Edit";
        public const string Pages_TaskStatuses_Delete = "Pages.TaskStatuses.Delete";

        public const string Pages_Severities = "Pages.Severities";
        public const string Pages_Severities_Create = "Pages.Severities.Create";
        public const string Pages_Severities_Edit = "Pages.Severities.Edit";
        public const string Pages_Severities_Delete = "Pages.Severities.Delete";

        //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

        public const string Pages = "Pages";

        public const string Pages_DemoUiComponents = "Pages.DemoUiComponents";
        public const string Pages_Administration = "Pages.Administration";

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_ChangePermissions = "Pages.Administration.Users.ChangePermissions";
        public const string Pages_Administration_Users_Impersonation = "Pages.Administration.Users.Impersonation";
        public const string Pages_Administration_Users_Unlock = "Pages.Administration.Users.Unlock";

        public const string Pages_Administration_Languages = "Pages.Administration.Languages";
        public const string Pages_Administration_Languages_Create = "Pages.Administration.Languages.Create";
        public const string Pages_Administration_Languages_Edit = "Pages.Administration.Languages.Edit";
        public const string Pages_Administration_Languages_Delete = "Pages.Administration.Languages.Delete";
        public const string Pages_Administration_Languages_ChangeTexts = "Pages.Administration.Languages.ChangeTexts";
        public const string Pages_Administration_Languages_ChangeDefaultLanguage = "Pages.Administration.Languages.ChangeDefaultLanguage";

        public const string Pages_Administration_AuditLogs = "Pages.Administration.AuditLogs";

        public const string Pages_Administration_OrganizationUnits = "Pages.Administration.OrganizationUnits";
        public const string Pages_Administration_OrganizationUnits_ManageOrganizationTree = "Pages.Administration.OrganizationUnits.ManageOrganizationTree";
        public const string Pages_Administration_OrganizationUnits_ManageMembers = "Pages.Administration.OrganizationUnits.ManageMembers";
        public const string Pages_Administration_OrganizationUnits_ManageRoles = "Pages.Administration.OrganizationUnits.ManageRoles";

        public const string Pages_Administration_HangfireDashboard = "Pages.Administration.HangfireDashboard";

        public const string Pages_Administration_UiCustomization = "Pages.Administration.UiCustomization";

        public const string Pages_Administration_WebhookSubscription = "Pages.Administration.WebhookSubscription";
        public const string Pages_Administration_WebhookSubscription_Create = "Pages.Administration.WebhookSubscription.Create";
        public const string Pages_Administration_WebhookSubscription_Edit = "Pages.Administration.WebhookSubscription.Edit";
        public const string Pages_Administration_WebhookSubscription_ChangeActivity = "Pages.Administration.WebhookSubscription.ChangeActivity";
        public const string Pages_Administration_WebhookSubscription_Detail = "Pages.Administration.WebhookSubscription.Detail";
        public const string Pages_Administration_Webhook_ListSendAttempts = "Pages.Administration.Webhook.ListSendAttempts";
        public const string Pages_Administration_Webhook_ResendWebhook = "Pages.Administration.Webhook.ResendWebhook";

        public const string Pages_Administration_DynamicProperties = "Pages.Administration.DynamicProperties";
        public const string Pages_Administration_DynamicProperties_Create = "Pages.Administration.DynamicProperties.Create";
        public const string Pages_Administration_DynamicProperties_Edit = "Pages.Administration.DynamicProperties.Edit";
        public const string Pages_Administration_DynamicProperties_Delete = "Pages.Administration.DynamicProperties.Delete";

        public const string Pages_Administration_DynamicPropertyValue = "Pages.Administration.DynamicPropertyValue";
        public const string Pages_Administration_DynamicPropertyValue_Create = "Pages.Administration.DynamicPropertyValue.Create";
        public const string Pages_Administration_DynamicPropertyValue_Edit = "Pages.Administration.DynamicPropertyValue.Edit";
        public const string Pages_Administration_DynamicPropertyValue_Delete = "Pages.Administration.DynamicPropertyValue.Delete";

        public const string Pages_Administration_DynamicEntityProperties = "Pages.Administration.DynamicEntityProperties";
        public const string Pages_Administration_DynamicEntityProperties_Create = "Pages.Administration.DynamicEntityProperties.Create";
        public const string Pages_Administration_DynamicEntityProperties_Edit = "Pages.Administration.DynamicEntityProperties.Edit";
        public const string Pages_Administration_DynamicEntityProperties_Delete = "Pages.Administration.DynamicEntityProperties.Delete";

        public const string Pages_Administration_DynamicEntityPropertyValue = "Pages.Administration.DynamicEntityPropertyValue";
        public const string Pages_Administration_DynamicEntityPropertyValue_Create = "Pages.Administration.DynamicEntityPropertyValue.Create";
        public const string Pages_Administration_DynamicEntityPropertyValue_Edit = "Pages.Administration.DynamicEntityPropertyValue.Edit";
        public const string Pages_Administration_DynamicEntityPropertyValue_Delete = "Pages.Administration.DynamicEntityPropertyValue.Delete";
        //TENANT-SPECIFIC PERMISSIONS

        public const string Pages_Tenant_Dashboard = "Pages.Tenant.Dashboard";

        public const string Pages_Administration_Tenant_Settings = "Pages.Administration.Tenant.Settings";

        public const string Pages_Administration_Tenant_SubscriptionManagement = "Pages.Administration.Tenant.SubscriptionManagement";

        //HOST-SPECIFIC PERMISSIONS

        public const string Pages_Editions = "Pages.Editions";
        public const string Pages_Editions_Create = "Pages.Editions.Create";
        public const string Pages_Editions_Edit = "Pages.Editions.Edit";
        public const string Pages_Editions_Delete = "Pages.Editions.Delete";
        public const string Pages_Editions_MoveTenantsToAnotherEdition = "Pages.Editions.MoveTenantsToAnotherEdition";

        public const string Pages_Tenants = "Pages.Tenants";
        public const string Pages_Tenants_Create = "Pages.Tenants.Create";
        public const string Pages_Tenants_Edit = "Pages.Tenants.Edit";
        public const string Pages_Tenants_ChangeFeatures = "Pages.Tenants.ChangeFeatures";
        public const string Pages_Tenants_Delete = "Pages.Tenants.Delete";
        public const string Pages_Tenants_Impersonation = "Pages.Tenants.Impersonation";

        public const string Pages_Administration_Host_Maintenance = "Pages.Administration.Host.Maintenance";
        public const string Pages_Administration_Host_Settings = "Pages.Administration.Host.Settings";
        public const string Pages_Administration_Host_Dashboard = "Pages.Administration.Host.Dashboard";

    }
}