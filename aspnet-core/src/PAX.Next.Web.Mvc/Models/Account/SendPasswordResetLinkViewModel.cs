using System.ComponentModel.DataAnnotations;

namespace PAX.Next.Web.Models.Account
{
    public class SendPasswordResetLinkViewModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}