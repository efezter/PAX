using System;
using Abp.Application.Services.Dto;

namespace PAX.Next.TaskManager.Dtos
{
    public class ReportTopStat : EntityDto
    {
        public string TaskStatusName { get; set; }
        public int Count { get; set; }
        public int Percentage { get; set; }
    }
}