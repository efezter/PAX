using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using PAX.TaskManager.Authorization.Users;
using PAX.TaskManager.MultiTenancy;

namespace PAX.TaskManager.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}