using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PAX.Next.MultiTenancy.HostDashboard.Dto;

namespace PAX.Next.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}