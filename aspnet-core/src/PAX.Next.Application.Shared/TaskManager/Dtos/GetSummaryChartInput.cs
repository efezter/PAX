using Abp.Application.Services.Dto;
using System;
using static PAX.Next.TaskManager.Utils.Enums;

namespace PAX.Next.TaskManager.Dtos
{
    public class GetSummaryChartInput
    {
        public SummaryChartType summaryChartType { get; set; }

    }
}