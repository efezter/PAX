using Abp.AspNetCore.Mvc.ViewComponents;

namespace PAX.TaskManager.Web.Views
{
    public abstract class TaskManagerViewComponent : AbpViewComponent
    {
        protected TaskManagerViewComponent()
        {
            LocalizationSourceName = TaskManagerConsts.LocalizationSourceName;
        }
    }
}