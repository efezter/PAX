using Abp.Events.Bus;

namespace PAX.TaskManager.MultiTenancy
{
    public class RecurringPaymentsEnabledEventData : EventData
    {
        public int TenantId { get; set; }
    }
}