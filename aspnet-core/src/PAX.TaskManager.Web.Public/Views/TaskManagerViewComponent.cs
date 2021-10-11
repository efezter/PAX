using Abp.AspNetCore.Mvc.ViewComponents;

namespace PAX.TaskManager.Web.Public.Views
{
    public abstract class TaskManagerViewComponent : AbpViewComponent
    {
        protected TaskManagerViewComponent()
        {
            LocalizationSourceName = TaskManagerConsts.LocalizationSourceName;
        }
    }
}