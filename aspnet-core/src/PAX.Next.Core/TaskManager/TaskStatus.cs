using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace PAX.Next.TaskManager
{
    [Table("TaskStatuses")]
    [Audited]
    public class TaskStatus : Entity
    {

        [Required]
        [StringLength(TaskStatusConsts.MaxNameLength, MinimumLength = TaskStatusConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(TaskStatusConsts.MaxIconUrlLength, MinimumLength = TaskStatusConsts.MinIconUrlLength)]
        public virtual string IconUrl { get; set; }

    }
}