using System.Threading.Tasks;
using Abp.Application.Services;
using PAX.Next.Sessions.Dto;

namespace PAX.Next.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
