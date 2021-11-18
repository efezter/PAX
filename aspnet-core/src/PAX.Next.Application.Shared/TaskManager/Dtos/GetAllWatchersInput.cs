using Abp.Application.Services.Dto;
using System;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetAllWatchersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string PaxTaskHeaderFilter { get; set; }

        public string UserNameFilter { get; set; }

    }
}