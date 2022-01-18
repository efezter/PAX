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
    [Table("TaskLabels")]
    [Audited]
    public class TaskLabel : Entity
    {

        public virtual int PaxTaskId { get; set; }

        [ForeignKey("PaxTaskId")]
        public PaxTask PaxTaskFk { get; set; }

        public virtual int LabelId { get; set; }

        [ForeignKey("LabelId")]
        public Label LabelFk { get; set; }

    }
}