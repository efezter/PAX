using System.Collections.Generic;
using PAX.Next.TaskManager.Dtos;
using PAX.Next.Dto;

namespace PAX.Next.TaskManager.Exporting
{
    public interface ISeveritiesExcelExporter
    {
        FileDto ExportToFile(List<GetSeverityForViewDto> severities);
    }
}