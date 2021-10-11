using Abp.AspNetZeroCore;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;
using PAX.TaskManager.Configuration;
using PAX.TaskManager.EntityFrameworkCore;
using PAX.TaskManager.Migrator.DependencyInjection;

namespace PAX.TaskManager.Migrator
{
    [DependsOn(typeof(TaskManagerEntityFrameworkCoreModule))]
    public class TaskManagerMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public TaskManagerMigratorModule(TaskManagerEntityFrameworkCoreModule taskManagerEntityFrameworkCoreModule)
        {
            taskManagerEntityFrameworkCoreModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(TaskManagerMigratorModule).GetAssembly().GetDirectoryPathOrNull(),
                addUserSecrets: true
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                TaskManagerConsts.ConnectionStringName
                );
            Configuration.Modules.AspNetZero().LicenseCode = _appConfiguration["AbpZeroLicenseCode"];

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(typeof(IEventBus), () =>
            {
                IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                );
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TaskManagerMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}