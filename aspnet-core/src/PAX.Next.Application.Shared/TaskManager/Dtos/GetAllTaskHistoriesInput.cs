using Abp.Application.Services.Dto;
using System;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetAllTaskHistoriesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int PaxTaskIdFilter { get; set; }

        public long UserIdFilter { get; set; }

    }
}