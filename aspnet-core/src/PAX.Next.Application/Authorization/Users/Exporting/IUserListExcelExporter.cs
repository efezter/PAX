using System.Collections.Generic;
using PAX.Next.Authorization.Users.Dto;
using PAX.Next.Dto;

namespace PAX.Next.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}