using Abp.Auditing;
using Abp.Domain.Entities.Auditing;
using PAX.Next.Authorization.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAX.Next.TaskManager
{
    [Table("Comments")]
    [Audited]
    public class Comment : FullAuditedEntity
    {

        public virtual string CommentText { get; set; }

        [DisableAuditing]
        public virtual int PaxTaskId { get; set; }

        [ForeignKey("PaxTaskId")]
        public PaxTask PaxTaskFk { get; set; }

        [DisableAuditing]
        public virtual long UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

    }
}