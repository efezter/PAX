using PAX.Next.TaskManager;
using PAX.Next.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Events.Bus.Entities;

namespace PAX.Next.TaskManager
{
    [Table("TaskHistories")]
    public class TaskHistory : Entity
    {
        public virtual string OldValue { get; set; }

        public virtual string NewValue { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 2)]
        public virtual string FieldName { get; set; }

        [Required]
        public virtual EntityChangeType ChangeType { get; set; }

        public virtual int? PaxTaskId { get; set; }

        [ForeignKey("PaxTaskId")]
        public PaxTask PaxTaskFk { get; set; }

        public virtual long CreatedUser { get; set; }

        [ForeignKey("CreatedUser")]
        public User CreatedUserFk { get; set; }

        [Required]
        public virtual DateTime CreatedDate { get; set; }
    }
}