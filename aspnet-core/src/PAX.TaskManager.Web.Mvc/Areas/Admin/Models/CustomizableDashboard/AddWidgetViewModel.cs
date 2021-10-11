using System.Collections.Generic;
using PAX.TaskManager.DashboardCustomization.Dto;

namespace PAX.TaskManager.Web.Areas.Admin.Models.CustomizableDashboard
{
    public class AddWidgetViewModel
    {
        public List<WidgetOutput> Widgets { get; set; }

        public string DashboardName { get; set; }

        public string PageId { get; set; }
    }
}
