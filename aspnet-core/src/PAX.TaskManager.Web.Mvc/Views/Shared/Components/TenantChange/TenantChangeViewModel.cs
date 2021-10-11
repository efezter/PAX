using Abp.AutoMapper;
using PAX.TaskManager.Sessions.Dto;

namespace PAX.TaskManager.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}