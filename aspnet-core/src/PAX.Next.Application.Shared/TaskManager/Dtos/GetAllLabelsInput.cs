using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetAllLabelsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public IEnumerable<long> OmitIds { get; set; }
    }
}