using Abp.Configuration;

namespace PAX.TaskManager.Timing.Dto
{
    public class GetTimezoneComboboxItemsInput
    {
        public SettingScopes DefaultTimezoneScope;

        public string SelectedTimezoneId { get; set; }
    }
}
