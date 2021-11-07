using System.Collections.Generic;
using Abp;
using PAX.Next.Chat.Dto;
using PAX.Next.Dto;

namespace PAX.Next.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
