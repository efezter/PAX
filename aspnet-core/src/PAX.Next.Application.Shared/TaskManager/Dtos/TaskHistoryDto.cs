using System;
using Abp.Application.Services.Dto;
using Abp.Events.Bus.Entities;

namespace PAX.Next.TaskManager.Dtos
{
    public class TaskHistoryDto : EntityDto
    {
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public string FieldName { get; set; }

        public int? PaxTaskId { get; set; }

        public EntityChangeType ChangeType { get; set; }

        public DateTime CreatedTime { get; set; }

        public long CreatedUser { get; set; }

    }
}