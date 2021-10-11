using System.ComponentModel.DataAnnotations;

namespace PAX.TaskManager.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
