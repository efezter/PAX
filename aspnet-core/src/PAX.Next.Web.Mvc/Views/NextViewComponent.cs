using Abp.AspNetCore.Mvc.ViewComponents;

namespace PAX.Next.Web.Views
{
    public abstract class NextViewComponent : AbpViewComponent
    {
        protected NextViewComponent()
        {
            LocalizationSourceName = NextConsts.LocalizationSourceName;
        }
    }
}