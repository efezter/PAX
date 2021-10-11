using Microsoft.Extensions.Configuration;

namespace PAX.TaskManager.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
