using Abp.Modules;
using Abp.Reflection.Extensions;

namespace PAX.TaskManager
{
    [DependsOn(typeof(TaskManagerCoreSharedModule))]
    public class TaskManagerApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TaskManagerApplicationSharedModule).GetAssembly());
        }
    }
}