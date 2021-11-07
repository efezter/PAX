using Abp.AutoMapper;
using PAX.Next.MultiTenancy;
using PAX.Next.MultiTenancy.Dto;
using PAX.Next.Web.Areas.Admin.Models.Common;

namespace PAX.Next.Web.Areas.Admin.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }
    }
}