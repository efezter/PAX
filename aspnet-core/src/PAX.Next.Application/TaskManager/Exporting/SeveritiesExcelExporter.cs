using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using PAX.Next.DataExporting.Excel.NPOI;
using PAX.Next.TaskManager.Dtos;
using PAX.Next.Dto;
using PAX.Next.Storage;

namespace PAX.Next.TaskManager.Exporting
{
    public class SeveritiesExcelExporter : NpoiExcelExporterBase, ISeveritiesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SeveritiesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSeverityForViewDto> severities)
        {
            return CreateExcelPackage(
                "Severities.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Severities"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Order"),
                        L("InsertedDate")
                        );

                    AddObjects(
                        sheet, severities,
                        _ => _.Severity.Name,
                        _ => _.Severity.Order,
                        _ => _timeZoneConverter.Convert(_.Severity.InsertedDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

                    for (var i = 1; i <= severities.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3);
                });
        }
    }
}