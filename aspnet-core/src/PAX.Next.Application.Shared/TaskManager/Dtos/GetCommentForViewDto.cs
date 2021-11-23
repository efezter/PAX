using System;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetCommentForViewDto
    {
        public CommentDto Comment { get; set; }

        public string PaxTaskHeader { get; set; }

        public DateTime CreationTime { get; set; }

        public string UserName { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
}