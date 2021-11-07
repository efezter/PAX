using System.Collections.Generic;
using PAX.Next.DashboardCustomization.Dto;

namespace PAX.Next.Web.Areas.Admin.Models.CustomizableDashboard
{
    public class AddWidgetViewModel
    {
        public List<WidgetOutput> Widgets { get; set; }

        public string DashboardName { get; set; }

        public string PageId { get; set; }
    }
}
