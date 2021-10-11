using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PAX.TaskManager.MultiTenancy.HostDashboard.Dto;

namespace PAX.TaskManager.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}