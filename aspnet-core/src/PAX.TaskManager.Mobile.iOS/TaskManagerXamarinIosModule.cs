using Abp.Modules;
using Abp.Reflection.Extensions;

namespace PAX.TaskManager
{
    [DependsOn(typeof(TaskManagerXamarinSharedModule))]
    public class TaskManagerXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TaskManagerXamarinIosModule).GetAssembly());
        }
    }
}