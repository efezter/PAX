using System.Collections.Generic;
using PAX.TaskManager.Auditing.Dto;
using PAX.TaskManager.Dto;

namespace PAX.TaskManager.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
