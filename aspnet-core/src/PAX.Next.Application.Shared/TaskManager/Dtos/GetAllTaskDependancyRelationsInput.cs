using Abp.Application.Services.Dto;
using System;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetAllTaskDependancyRelationsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int TaskIdFilter { get; set; }

        public string PaxTaskHeaderFilter { get; set; }

        public string PaxTaskHeader2Filter { get; set; }

    }
}