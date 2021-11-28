using Abp.Application.Services.Dto;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetUsersForMention
    {
        public string Id { get; set; }

        public long UserId { get; set; }

        public string DisplayName { get; set; }
    }
}