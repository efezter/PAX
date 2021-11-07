using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using PAX.Next.DataExporting.Excel.NPOI;
using PAX.Next.TaskManager.Dtos;
using PAX.Next.Dto;
using PAX.Next.Storage;

namespace PAX.Next.TaskManager.Exporting
{
    public class TaskStatusesExcelExporter : NpoiExcelExporterBase, ITaskStatusesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TaskStatusesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTaskStatusForViewDto> taskStatuses)
        {
            return CreateExcelPackage(
                "TaskStatuses.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("TaskStatuses"));

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, taskStatuses,
                        _ => _.TaskStatus.Name
                        );

                });
        }
    }
}