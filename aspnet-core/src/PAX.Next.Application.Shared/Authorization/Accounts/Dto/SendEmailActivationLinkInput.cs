using System.ComponentModel.DataAnnotations;

namespace PAX.Next.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}