using System.Collections.Generic;
using PAX.TaskManager.Authorization.Users.Importing.Dto;
using PAX.TaskManager.Dto;

namespace PAX.TaskManager.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
