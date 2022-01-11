using PAX.Next.TaskManager;
using System;
using System.Linq;
using Abp.Organizations;
using PAX.Next.Authorization.Roles;
using PAX.Next.MultiTenancy;

namespace PAX.Next.EntityHistory
{
    public static class EntityHistoryHelper
    {
        public const string EntityHistoryConfigurationName = "EntityHistory";

        public static readonly Type[] HostSideTrackedTypes =
        {
            typeof(OrganizationUnit), typeof(Role), typeof(Tenant)
        };

        public static readonly Type[] TenantSideTrackedTypes =
        {
            typeof(OrganizationUnit), typeof(Role)
        };

        public static readonly Type[] TaskManagerTrackedTypes =
        {
            typeof(PaxTask), typeof(PaxTaskAttachment), typeof(Comment), typeof(Watcher)
        };

        public static readonly Type[] TrackedTypes =
            HostSideTrackedTypes
                //.Concat(TenantSideTrackedTypes)
                .Concat(TaskManagerTrackedTypes)
                .GroupBy(type => type.FullName)
                .Select(types => types.First())
                .ToArray();
    }
}