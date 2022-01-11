﻿using Abp.Auditing;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAX.Next.TaskManager
{
    [Table("PaxTaskAttachments")]
    [Audited]
    public class PaxTaskAttachment : FullAuditedEntity
    {

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public virtual string FileName { get; set; }

        public virtual int PaxTaskId { get; set; }

        [ForeignKey("PaxTaskId")]
        public PaxTask PaxTaskFk { get; set; }

    }
}