using System.Threading.Tasks;

namespace PAX.TaskManager.Security
{
    public interface IPasswordComplexitySettingStore
    {
        Task<PasswordComplexitySetting> GetSettingsAsync();
    }
}
