using System.Threading.Tasks;
using Abp.Application.Services;
using PAX.TaskManager.Editions.Dto;
using PAX.TaskManager.MultiTenancy.Dto;

namespace PAX.TaskManager.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}