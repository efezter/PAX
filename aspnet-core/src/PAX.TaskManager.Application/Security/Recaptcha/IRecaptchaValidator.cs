using System.Threading.Tasks;

namespace PAX.TaskManager.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}