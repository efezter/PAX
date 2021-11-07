using System.Threading.Tasks;
using Abp.Application.Services;
using PAX.Next.Editions.Dto;
using PAX.Next.MultiTenancy.Dto;

namespace PAX.Next.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}