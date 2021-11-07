using System.Collections.Generic;
using PAX.Next.Authorization.Users.Importing.Dto;
using PAX.Next.Dto;

namespace PAX.Next.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
