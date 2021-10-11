using System.Threading.Tasks;
using Abp.Application.Services;
using PAX.TaskManager.Sessions.Dto;

namespace PAX.TaskManager.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
