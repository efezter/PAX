using System.ComponentModel.DataAnnotations;

namespace PAX.TaskManager.Web.Models.Account
{
    public class SendPasswordResetLinkViewModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}