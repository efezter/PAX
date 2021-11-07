using System.Threading.Tasks;
using PAX.Next.Sessions.Dto;

namespace PAX.Next.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
