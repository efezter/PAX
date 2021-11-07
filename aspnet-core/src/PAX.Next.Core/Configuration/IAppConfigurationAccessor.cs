using Microsoft.Extensions.Configuration;

namespace PAX.Next.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
