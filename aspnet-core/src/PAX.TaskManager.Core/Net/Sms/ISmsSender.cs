using System.Threading.Tasks;

namespace PAX.TaskManager.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}