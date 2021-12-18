using Abp.Application.Services.Dto;
using System;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetAllPaxTaskAttachmentsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int TaskId { get; set; }

        public DateTime CreationTime { get; set; }

        public string UserName { get; set; }
    }
}