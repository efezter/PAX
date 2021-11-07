using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PAX.Next.EntityFrameworkCore;

namespace PAX.Next.HealthChecks
{
    public class NextDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public NextDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("NextDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("NextDbContext could not connect to database"));
        }
    }
}
