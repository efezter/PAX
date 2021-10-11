using Abp.AutoMapper;
using PAX.TaskManager.MultiTenancy.Dto;

namespace PAX.TaskManager.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(RegisterTenantOutput))]
    public class TenantRegisterResultViewModel : RegisterTenantOutput
    {
        public string TenantLoginAddress { get; set; }
    }
}