using Abp.AspNetCore.Mvc.Views;

namespace PAX.TaskManager.Web.Views
{
    public abstract class TaskManagerRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected TaskManagerRazorPage()
        {
            LocalizationSourceName = TaskManagerConsts.LocalizationSourceName;
        }
    }
}
