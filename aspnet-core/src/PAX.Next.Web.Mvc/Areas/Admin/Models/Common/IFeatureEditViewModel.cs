using System.Collections.Generic;
using Abp.Application.Services.Dto;
using PAX.Next.Editions.Dto;

namespace PAX.Next.Web.Areas.Admin.Models.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        List<FlatFeatureDto> Features { get; set; }
    }
}