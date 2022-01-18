using Abp.Application.Services.Dto;
using System;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetAllTaskLabelsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string PaxTaskHeaderFilter { get; set; }

        public string LabelNameFilter { get; set; }

    }
}