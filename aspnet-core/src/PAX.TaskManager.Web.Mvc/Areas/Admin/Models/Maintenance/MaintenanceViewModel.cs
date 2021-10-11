using System.Collections.Generic;
using PAX.TaskManager.Caching.Dto;

namespace PAX.TaskManager.Web.Areas.Admin.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}