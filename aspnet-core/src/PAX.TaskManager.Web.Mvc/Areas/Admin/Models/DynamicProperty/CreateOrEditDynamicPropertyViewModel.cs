using System.Collections.Generic;
using PAX.TaskManager.DynamicEntityProperties.Dto;

namespace PAX.TaskManager.Web.Areas.Admin.Models.DynamicProperty
{
    public class CreateOrEditDynamicPropertyViewModel
    {
        public DynamicPropertyDto DynamicPropertyDto { get; set; }

        public List<string> AllowedInputTypes { get; set; }
    }
}
