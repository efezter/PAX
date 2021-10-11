using Abp.Domain.Services;

namespace PAX.TaskManager
{
    public abstract class TaskManagerDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected TaskManagerDomainServiceBase()
        {
            LocalizationSourceName = TaskManagerConsts.LocalizationSourceName;
        }
    }
}
