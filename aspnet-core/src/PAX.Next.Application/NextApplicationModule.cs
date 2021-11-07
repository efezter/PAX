using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using PAX.Next.Authorization;

namespace PAX.Next
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(NextApplicationSharedModule),
        typeof(NextCoreModule)
        )]
    public class NextApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NextApplicationModule).GetAssembly());
        }
    }
}