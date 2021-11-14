using Abp.Application.Services.Dto;
using System;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetAllPaxTasksInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string HeaderFilter { get; set; }

        public DateTime? MaxCreatedDateFilter { get; set; }
        public DateTime? MinCreatedDateFilter { get; set; }

        public int? TaskTypeFilter { get; set; }

        public int? TaskTypePeriodFilter { get; set; }

        public int? MaxPeriodIntervalFilter { get; set; }
        public int? MinPeriodIntervalFilter { get; set; }

        public string UserNameFilter { get; set; }

        public string UserName2Filter { get; set; }

        public string SeverityNameFilter { get; set; }

        public string TaskStatusNameFilter { get; set; }

    }
}