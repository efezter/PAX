using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PAX.TaskManager.EntityFrameworkCore;

namespace PAX.TaskManager.HealthChecks
{
    public class TaskManagerDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public TaskManagerDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("TaskManagerDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("TaskManagerDbContext could not connect to database"));
        }
    }
}
