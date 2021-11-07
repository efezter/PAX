using System.Collections.Generic;
using PAX.Next.Caching.Dto;

namespace PAX.Next.Web.Areas.Admin.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}