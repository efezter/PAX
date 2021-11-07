using Abp.Modules;
using Abp.Reflection.Extensions;

namespace PAX.Next
{
    [DependsOn(typeof(NextXamarinSharedModule))]
    public class NextXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NextXamarinAndroidModule).GetAssembly());
        }
    }
}