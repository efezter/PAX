using System.ComponentModel.DataAnnotations;

namespace PAX.Next.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}