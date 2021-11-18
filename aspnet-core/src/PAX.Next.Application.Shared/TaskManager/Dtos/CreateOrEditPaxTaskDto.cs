using PAX.Next.TaskManager.Utils;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PAX.Next.TaskManager.Dtos
{
    public class CreateOrEditPaxTaskDto : EntityDto<int?>
    {

        [Required]
        [StringLength(PaxTaskConsts.MaxHeaderLength, MinimumLength = PaxTaskConsts.MinHeaderLength)]
        public string Header { get; set; }

        public string Details { get; set; }

        public DateTime CreatedDate { get; set; }

        public Enums.TaskType TaskType { get; set; }

        public Enums.TaskTypePeriod TaskTypePeriod { get; set; }

        public int? PeriodInterval { get; set; }

        public long ReporterId { get; set; }

        public long? AssigneeId { get; set; }

        public int? SeverityId { get; set; }

        public int TaskStatusId { get; set; }

        public IEnumerable<WatcherUserLookupTableDto> Watchers { get; set; }

    }
}