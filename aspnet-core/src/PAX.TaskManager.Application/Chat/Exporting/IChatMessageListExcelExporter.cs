using System.Collections.Generic;
using Abp;
using PAX.TaskManager.Chat.Dto;
using PAX.TaskManager.Dto;

namespace PAX.TaskManager.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
