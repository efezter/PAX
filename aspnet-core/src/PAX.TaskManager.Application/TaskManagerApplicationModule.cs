using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using PAX.TaskManager.Authorization;

namespace PAX.TaskManager
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(TaskManagerApplicationSharedModule),
        typeof(TaskManagerCoreModule)
        )]
    public class TaskManagerApplicationModule : AbpModule
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
            IocManager.RegisterAssemblyByConvention(typeof(TaskManagerApplicationModule).GetAssembly());
        }
    }
}