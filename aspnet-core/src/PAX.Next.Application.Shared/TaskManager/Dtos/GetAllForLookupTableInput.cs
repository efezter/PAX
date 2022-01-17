using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public IEnumerable<long> OmitUserIds { get; set; }
    }
}