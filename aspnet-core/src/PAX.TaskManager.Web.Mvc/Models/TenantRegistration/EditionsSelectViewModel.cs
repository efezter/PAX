using Abp.AutoMapper;
using PAX.TaskManager.MultiTenancy.Dto;

namespace PAX.TaskManager.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(EditionsSelectOutput))]
    public class EditionsSelectViewModel : EditionsSelectOutput
    {
    }
}
