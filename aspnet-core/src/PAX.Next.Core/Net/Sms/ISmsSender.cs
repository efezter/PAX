using System.Threading.Tasks;

namespace PAX.Next.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}