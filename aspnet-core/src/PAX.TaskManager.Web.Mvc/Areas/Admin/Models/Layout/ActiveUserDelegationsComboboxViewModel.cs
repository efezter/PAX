using System.Collections.Generic;
using PAX.TaskManager.Authorization.Delegation;
using PAX.TaskManager.Authorization.Users.Delegation.Dto;

namespace PAX.TaskManager.Web.Areas.Admin.Models.Layout
{
    public class ActiveUserDelegationsComboboxViewModel
    {
        public IUserDelegationConfiguration UserDelegationConfiguration { get; set; }
        
        public List<UserDelegationDto> UserDelegations { get; set; }
    }
}
