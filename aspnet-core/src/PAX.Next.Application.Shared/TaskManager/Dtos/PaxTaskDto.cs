using PAX.Next.TaskManager.Utils;

using System;
using Abp.Application.Services.Dto;

namespace PAX.Next.TaskManager.Dtos
{
    public class PaxTaskDto : EntityDto
    {
        public string Header { get; set; }

        public DateTime CreatedDate { get; set; }

        public Enums.TaskType TaskType { get; set; }

        public Enums.TaskTypePeriod TaskTypePeriod { get; set; }

        public int? PeriodInterval { get; set; }

        public long ReporterId { get; set; }

        public long? AssigneeId { get; set; }

        public int? SeverityId { get; set; }

        public int TaskStatusId { get; set; }

        public DateTime? DeadLineDate { get; set; }

    }
}