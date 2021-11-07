using System.Collections.Generic;
using PAX.Next.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace PAX.Next.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
