using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace PAX.TaskManager
{
    [DependsOn(typeof(TaskManagerClientModule), typeof(AbpAutoMapperModule))]
    public class TaskManagerXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TaskManagerXamarinSharedModule).GetAssembly());
        }
    }
}