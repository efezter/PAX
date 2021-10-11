using System.Collections.Generic;
using PAX.TaskManager.Organizations.Dto;

namespace PAX.TaskManager.Web.Areas.Admin.Models.Common
{
    public interface IOrganizationUnitsEditViewModel
    {
        List<OrganizationUnitDto> AllOrganizationUnits { get; set; }

        List<string> MemberedOrganizationUnits { get; set; }
    }
}