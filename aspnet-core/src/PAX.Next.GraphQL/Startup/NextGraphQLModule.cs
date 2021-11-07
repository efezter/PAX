using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace PAX.Next.Startup
{
    [DependsOn(typeof(NextCoreModule))]
    public class NextGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NextGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}