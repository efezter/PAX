﻿using PAX.Next.TaskManager;
using PAX.Next.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace PAX.Next.TaskManager
{
    [Table("Comments")]
    [Audited]
    public class Comment : FullAuditedEntity
    {

        public virtual string CommentText { get; set; }

        public virtual int PaxTaskId { get; set; }

        [ForeignKey("PaxTaskId")]
        public PaxTask PaxTaskFk { get; set; }

        public virtual long UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

    }
}