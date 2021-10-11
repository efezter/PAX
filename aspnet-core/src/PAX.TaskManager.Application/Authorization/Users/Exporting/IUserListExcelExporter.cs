using System.Collections.Generic;
using PAX.TaskManager.Authorization.Users.Dto;
using PAX.TaskManager.Dto;

namespace PAX.TaskManager.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}