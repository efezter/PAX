using Abp.AutoMapper;
using PAX.TaskManager.Localization.Dto;

namespace PAX.TaskManager.Web.Areas.Admin.Models.Languages
{
    [AutoMapFrom(typeof(GetLanguageForEditOutput))]
    public class CreateOrEditLanguageModalViewModel : GetLanguageForEditOutput
    {
        public bool IsEditMode => Language.Id.HasValue;
    }
}