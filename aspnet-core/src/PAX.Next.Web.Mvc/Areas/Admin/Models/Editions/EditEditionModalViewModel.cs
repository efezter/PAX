using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using PAX.Next.Editions.Dto;
using PAX.Next.Web.Areas.Admin.Models.Common;

namespace PAX.Next.Web.Areas.Admin.Models.Editions
{
    [AutoMapFrom(typeof(GetEditionEditOutput))]
    public class EditEditionModalViewModel : GetEditionEditOutput, IFeatureEditViewModel
    {
        public bool IsEditMode => Edition.Id.HasValue;

        public IReadOnlyList<ComboboxItemDto> EditionItems { get; set; }

        public IReadOnlyList<ComboboxItemDto> FreeEditionItems { get; set; }
    }
}