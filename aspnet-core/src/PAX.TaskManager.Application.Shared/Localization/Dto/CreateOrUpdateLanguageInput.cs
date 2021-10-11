using System.ComponentModel.DataAnnotations;

namespace PAX.TaskManager.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}