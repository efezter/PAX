using System;
using Abp.Application.Services.Dto;

namespace PAX.Next.TaskManager.Dtos
{
    public class CommentDto : EntityDto
    {
        public int CommentText { get; set; }

        public int PaxTaskId { get; set; }

        public long UserId { get; set; }

    }
}