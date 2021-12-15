using System;
using Abp.Application.Services.Dto;

namespace PAX.Next.TaskManager.Dtos
{
    public class PaxTaskAttachmentDto : EntityDto
    {
        public int Id { get; set; }

        public int PaxTaskId { get; set; }

        public string FileName { get; set; }

    }
}