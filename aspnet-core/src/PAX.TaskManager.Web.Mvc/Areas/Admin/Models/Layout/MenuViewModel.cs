using Abp.Application.Navigation;

namespace PAX.TaskManager.Web.Areas.Admin.Models.Layout
{
    public class MenuViewModel
    {
        public UserMenu Menu { get; set; }

        public string CurrentPageName { get; set; }
    }
}