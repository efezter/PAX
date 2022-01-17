using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using Abp.Events.Bus.Entities;

namespace PAX.Next.TaskManager.Dtos
{
    public class CreateOrEditTaskHistoryDto : EntityDto<int?>
    {

        [Required]
        public string OldValue { get; set; }

        [Required]
        public string FieldName { get; set; }

        [Required]
        public string NewValue { get; set; }

        [Required]
        public EntityChangeType ChangeType { get; set; }

        public int? PaxTaskId { get; set; }

        public long CreatedUser { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}