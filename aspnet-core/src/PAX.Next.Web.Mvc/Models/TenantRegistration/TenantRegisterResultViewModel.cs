using Abp.AutoMapper;
using PAX.Next.MultiTenancy.Dto;

namespace PAX.Next.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(RegisterTenantOutput))]
    public class TenantRegisterResultViewModel : RegisterTenantOutput
    {
        public string TenantLoginAddress { get; set; }
    }
}