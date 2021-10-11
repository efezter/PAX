using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace PAX.TaskManager.Startup
{
    [DependsOn(typeof(TaskManagerCoreModule))]
    public class TaskManagerGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TaskManagerGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}