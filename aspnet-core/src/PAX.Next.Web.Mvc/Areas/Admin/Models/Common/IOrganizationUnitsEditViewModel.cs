using System.Collections.Generic;
using PAX.Next.Organizations.Dto;

namespace PAX.Next.Web.Areas.Admin.Models.Common
{
    public interface IOrganizationUnitsEditViewModel
    {
        List<OrganizationUnitDto> AllOrganizationUnits { get; set; }

        List<string> MemberedOrganizationUnits { get; set; }
    }
}