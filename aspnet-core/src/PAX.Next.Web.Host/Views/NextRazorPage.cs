using Abp.AspNetCore.Mvc.Views;

namespace PAX.Next.Web.Views
{
    public abstract class NextRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected NextRazorPage()
        {
            LocalizationSourceName = NextConsts.LocalizationSourceName;
        }
    }
}
