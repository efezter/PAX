using Abp.AutoMapper;
using PAX.TaskManager.Organizations.Dto;

namespace PAX.TaskManager.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}