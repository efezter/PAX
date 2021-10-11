using System.Collections.Generic;
using PAX.TaskManager.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace PAX.TaskManager.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
