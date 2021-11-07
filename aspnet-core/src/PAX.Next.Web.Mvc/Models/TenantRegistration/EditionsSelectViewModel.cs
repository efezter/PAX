using Abp.AutoMapper;
using PAX.Next.MultiTenancy.Dto;

namespace PAX.Next.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(EditionsSelectOutput))]
    public class EditionsSelectViewModel : EditionsSelectOutput
    {
    }
}
