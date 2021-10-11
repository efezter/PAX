using System.Globalization;

namespace PAX.TaskManager.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}