using Abp.Application.Services.Dto;
using System;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetAllTaskStatusesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}