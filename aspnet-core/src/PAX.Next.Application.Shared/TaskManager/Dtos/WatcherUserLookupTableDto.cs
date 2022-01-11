using Abp.Application.Services.Dto;

namespace PAX.Next.TaskManager.Dtos
{
    public class WatcherUserLookupTableDto
    {
        public int Id { get; set; }

        public long UserId { get; set; }

        public string DisplayName { get; set; }
    }
}