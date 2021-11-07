using Abp.Modules;
using Abp.Reflection.Extensions;

namespace PAX.Next
{
    [DependsOn(typeof(NextCoreSharedModule))]
    public class NextApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NextApplicationSharedModule).GetAssembly());
        }
    }
}