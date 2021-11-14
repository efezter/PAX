using Abp.Application.Services.Dto;
using System;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetAllTagsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

    }
}