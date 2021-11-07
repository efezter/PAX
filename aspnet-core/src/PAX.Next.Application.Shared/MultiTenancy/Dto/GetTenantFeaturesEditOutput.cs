using System.Collections.Generic;
using Abp.Application.Services.Dto;
using PAX.Next.Editions.Dto;

namespace PAX.Next.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}