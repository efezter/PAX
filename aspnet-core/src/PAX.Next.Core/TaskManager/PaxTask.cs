using PAX.Next.TaskManager.Utils;
using PAX.Next.Authorization.Users;
using PAX.Next.TaskManager;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace PAX.Next.TaskManager
{
    [Table("PaxTasks")]
    [Audited]
    public class PaxTask : Entity
    {

        [Required]
        [StringLength(PaxTaskConsts.MaxHeaderLength, MinimumLength = PaxTaskConsts.MinHeaderLength)]
        public virtual string Header { get; set; }

        public virtual string Details { get; set; }

        [DisableAuditing]
        public virtual DateTime CreatedDate { get; set; }

        public virtual Enums.TaskType TaskType { get; set; }

        public virtual Enums.TaskTypePeriod TaskTypePeriod { get; set; }

        public virtual int? PeriodInterval { get; set; }

        [DisableAuditing]
        public virtual long ReporterId { get; set; }

        [ForeignKey("ReporterId")]
        public User ReporterFk { get; set; }

        public virtual long? AssigneeId { get; set; }

        [ForeignKey("AssigneeId")]
        public User AssigneeFk { get; set; }

        public virtual int? SeverityId { get; set; }

        [ForeignKey("SeverityId")]
        public Severity SeverityFk { get; set; }

        public virtual int TaskStatusId { get; set; }

        [ForeignKey("TaskStatusId")]
        public TaskStatus TaskStatusFk { get; set; }

    }
}