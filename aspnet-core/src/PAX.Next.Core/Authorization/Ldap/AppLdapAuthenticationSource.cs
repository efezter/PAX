using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using PAX.Next.Authorization.Users;
using PAX.Next.MultiTenancy;

namespace PAX.Next.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}