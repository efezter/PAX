using Abp.AspNetCore.Mvc.ViewComponents;

namespace PAX.Next.Web.Public.Views
{
    public abstract class NextViewComponent : AbpViewComponent
    {
        protected NextViewComponent()
        {
            LocalizationSourceName = NextConsts.LocalizationSourceName;
        }
    }
}