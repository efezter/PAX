using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace PAX.Next.Web.Areas.Admin.Models.Users
{
    public class UserLoginAttemptsViewModel
    {
        public List<ComboboxItemDto> LoginAttemptResults { get; set; }
    }
}