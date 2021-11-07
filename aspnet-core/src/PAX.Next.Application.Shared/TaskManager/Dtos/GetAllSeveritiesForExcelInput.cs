using Abp.Application.Services.Dto;
using System;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetAllSeveritiesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public int? MaxOrderFilter { get; set; }
        public int? MinOrderFilter { get; set; }

        public DateTime? MaxInsertedDateFilter { get; set; }
        public DateTime? MinInsertedDateFilter { get; set; }

    }
}