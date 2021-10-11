using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace PAX.TaskManager.Web.Public.Views
{
    public abstract class TaskManagerRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected TaskManagerRazorPage()
        {
            LocalizationSourceName = TaskManagerConsts.LocalizationSourceName;
        }
    }
}
