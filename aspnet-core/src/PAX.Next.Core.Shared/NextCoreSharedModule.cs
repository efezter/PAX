using Abp.Modules;
using Abp.Reflection.Extensions;

namespace PAX.Next
{
    public class NextCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NextCoreSharedModule).GetAssembly());
        }
    }
}