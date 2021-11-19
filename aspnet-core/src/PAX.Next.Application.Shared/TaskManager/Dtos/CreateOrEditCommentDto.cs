using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace PAX.Next.TaskManager.Dtos
{
    public class CreateOrEditCommentDto : EntityDto<int?>
    {

        public int PaxTaskId { get; set; }

        public string CommentText { get; set; }

        public long UserId { get; set; }

    }
}