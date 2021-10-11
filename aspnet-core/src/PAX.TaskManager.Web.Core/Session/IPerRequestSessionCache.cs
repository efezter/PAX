using System.Threading.Tasks;
using PAX.TaskManager.Sessions.Dto;

namespace PAX.TaskManager.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
