using PAX.TaskManager.Dto;

namespace PAX.TaskManager.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}
