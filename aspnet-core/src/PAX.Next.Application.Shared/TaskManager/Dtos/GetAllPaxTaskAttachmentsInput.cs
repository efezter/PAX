using Abp.Application.Services.Dto;
using System;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetAllPaxTaskAttachmentsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string PaxTaskHeaderFilter { get; set; }

    }
}