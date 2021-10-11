using Abp.Modules;
using Abp.Reflection.Extensions;

namespace PAX.TaskManager
{
    public class TaskManagerCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TaskManagerCoreSharedModule).GetAssembly());
        }
    }
}