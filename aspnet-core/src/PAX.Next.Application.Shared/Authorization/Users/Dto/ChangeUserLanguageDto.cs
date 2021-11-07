using System.ComponentModel.DataAnnotations;

namespace PAX.Next.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
