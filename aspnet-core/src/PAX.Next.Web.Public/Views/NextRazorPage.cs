using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace PAX.Next.Web.Public.Views
{
    public abstract class NextRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected NextRazorPage()
        {
            LocalizationSourceName = NextConsts.LocalizationSourceName;
        }
    }
}
