using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace PAX.Next
{
    [DependsOn(typeof(NextClientModule), typeof(AbpAutoMapperModule))]
    public class NextXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NextXamarinSharedModule).GetAssembly());
        }
    }
}