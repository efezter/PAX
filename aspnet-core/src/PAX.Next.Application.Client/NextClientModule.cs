using Abp.Modules;
using Abp.Reflection.Extensions;

namespace PAX.Next
{
    public class NextClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NextClientModule).GetAssembly());
        }
    }
}
