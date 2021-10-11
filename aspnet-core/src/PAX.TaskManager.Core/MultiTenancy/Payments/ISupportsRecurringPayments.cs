using Abp.Events.Bus.Handlers;

namespace PAX.TaskManager.MultiTenancy.Payments
{
    public interface ISupportsRecurringPayments : 
        IEventHandler<RecurringPaymentsDisabledEventData>, 
        IEventHandler<RecurringPaymentsEnabledEventData>,
        IEventHandler<TenantEditionChangedEventData>
    {

    }
}
