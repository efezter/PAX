using System.Collections.Generic;
using PAX.Next.DynamicEntityProperties.Dto;

namespace PAX.Next.Web.Areas.Admin.Models.DynamicEntityProperty
{
    public class CreateEntityDynamicPropertyViewModel
    {
        public string EntityFullName { get; set; }

        public List<string> AllEntities { get; set; }

        public List<DynamicPropertyDto> DynamicProperties { get; set; }
    }
}
