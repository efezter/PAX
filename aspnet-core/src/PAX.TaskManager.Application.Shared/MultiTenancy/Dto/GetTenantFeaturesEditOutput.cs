using System.Collections.Generic;
using Abp.Application.Services.Dto;
using PAX.TaskManager.Editions.Dto;

namespace PAX.TaskManager.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}