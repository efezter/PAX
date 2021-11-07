using Abp.Domain.Services;

namespace PAX.Next
{
    public abstract class NextDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected NextDomainServiceBase()
        {
            LocalizationSourceName = NextConsts.LocalizationSourceName;
        }
    }
}
