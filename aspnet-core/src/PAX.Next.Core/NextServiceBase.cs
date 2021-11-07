using Abp;

namespace PAX.Next
{
    /// <summary>
    /// This class can be used as a base class for services in this application.
    /// It has some useful objects property-injected and has some basic methods most of services may need to.
    /// It's suitable for non domain nor application service classes.
    /// For domain services inherit <see cref="NextDomainServiceBase"/>.
    /// For application services inherit NextAppServiceBase.
    /// </summary>
    public abstract class NextServiceBase : AbpServiceBase
    {
        protected NextServiceBase()
        {
            LocalizationSourceName = NextConsts.LocalizationSourceName;
        }
    }
}