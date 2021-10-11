using Abp.AutoMapper;
using PAX.TaskManager.MultiTenancy;
using PAX.TaskManager.MultiTenancy.Dto;
using PAX.TaskManager.Web.Areas.Admin.Models.Common;

namespace PAX.TaskManager.Web.Areas.Admin.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }
    }
}