using PAX.Next.TaskManager;
using PAX.Next.TaskManager;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace PAX.Next.TaskManager
{
    [Table("TaskDependancyRelations")]
    [Audited]
    public class TaskDependancyRelation : Entity
    {

        public virtual int PaxTaskId { get; set; }

        [ForeignKey("PaxTaskId")]
        public PaxTask PaxTaskFk { get; set; }

        public virtual int DependOnTaskId { get; set; }

        [ForeignKey("DependOnTaskId")]
        public PaxTask DependOnTaskFk { get; set; }

    }
}