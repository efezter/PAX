using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace PAX.Next.TaskManager
{
    [Table("Labels")]
    [Audited]
    public class Label : Entity
    {

        [Required]
        [StringLength(LabelConsts.MaxNameLength, MinimumLength = LabelConsts.MinNameLength)]
        public virtual string Name { get; set; }

    }
}