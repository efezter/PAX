using Abp.Application.Services.Dto;

namespace PAX.Next.TaskManager.Dtos
{
    public class PaxTaskUserLookupTableDto
    {
        public long UserId { get; set; }

        public string DisplayName { get; set; }
    }
}