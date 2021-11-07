using System.Collections.Generic;
using PAX.Next.Auditing.Dto;
using PAX.Next.Dto;

namespace PAX.Next.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
