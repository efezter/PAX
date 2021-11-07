using Abp.Application.Services.Dto;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}