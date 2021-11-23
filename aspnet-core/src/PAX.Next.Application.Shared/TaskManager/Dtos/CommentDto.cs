using System;
using Abp.Application.Services.Dto;

namespace PAX.Next.TaskManager.Dtos
{
    public class CommentDto : EntityDto
    {
        public string CommentText { get; set; }

        public string UserName { get; set; }

        public int PaxTaskId { get; set; }

        public long UserId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}