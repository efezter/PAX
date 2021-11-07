using Abp.Application.Navigation;

namespace PAX.Next.Web.Areas.Admin.Models.Layout
{
    public class MenuViewModel
    {
        public UserMenu Menu { get; set; }

        public string CurrentPageName { get; set; }
    }
}