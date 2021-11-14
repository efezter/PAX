using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using PAX.Next.DataExporting.Excel.NPOI;
using PAX.Next.TaskManager.Dtos;
using PAX.Next.Dto;
using PAX.Next.Storage;

namespace PAX.Next.TaskManager.Exporting
{
    public class PaxTasksExcelExporter : NpoiExcelExporterBase, IPaxTasksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PaxTasksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPaxTaskForViewDto> paxTasks)
        {
            return CreateExcelPackage(
                "PaxTasks.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("PaxTasks"));

                    AddHeader(
                        sheet,
                        L("Header"),
                        L("CreatedDate"),
                        L("TaskType"),
                        L("TaskTypePeriod"),
                        L("PeriodInterval"),
                        (L("User")) + L("Name"),
                        (L("User")) + L("Name"),
                        (L("Severity")) + L("Name"),
                        (L("TaskStatus")) + L("Name")
                        );

                    AddObjects(
                        sheet, paxTasks,
                        _ => _.PaxTask.Header,
                        _ => _timeZoneConverter.Convert(_.PaxTask.CreatedDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.PaxTask.TaskType,
                        _ => _.PaxTask.TaskTypePeriod,
                        _ => _.PaxTask.PeriodInterval,
                        _ => _.UserName,
                        _ => _.UserName2,
                        _ => _.SeverityName,
                        _ => _.TaskStatusName
                        );

                    for (var i = 1; i <= paxTasks.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(2);
                });
        }
    }
}