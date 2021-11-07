using System.Collections.Generic;
using PAX.Next.DynamicEntityProperties.Dto;

namespace PAX.Next.Web.Areas.Admin.Models.DynamicProperty
{
    public class CreateOrEditDynamicPropertyViewModel
    {
        public DynamicPropertyDto DynamicPropertyDto { get; set; }

        public List<string> AllowedInputTypes { get; set; }
    }
}
