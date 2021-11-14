using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace PAX.Next.TaskManager
{
    [Table("Tags")]
    [Audited]
    public class Tag : Entity
    {

        [Required]
        [StringLength(TagConsts.MaxNameLength, MinimumLength = TagConsts.MinNameLength)]
        public virtual string Name { get; set; }

    }
}