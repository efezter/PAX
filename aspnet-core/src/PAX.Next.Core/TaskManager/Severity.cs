using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace PAX.Next.TaskManager
{
    [Table("Severities")]
    [Audited]
    public class Severity : Entity
    {

        [Required]
        [StringLength(SeverityConsts.MaxNameLength, MinimumLength = SeverityConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual int Order { get; set; }

        public virtual DateTime InsertedDate { get; set; }

    }
}