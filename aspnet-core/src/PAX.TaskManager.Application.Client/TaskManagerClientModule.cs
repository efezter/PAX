using Abp.Modules;
using Abp.Reflection.Extensions;

namespace PAX.TaskManager
{
    public class TaskManagerClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TaskManagerClientModule).GetAssembly());
        }
    }
}
