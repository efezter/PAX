using System.Collections.Generic;
using PAX.Next.Authorization.Delegation;
using PAX.Next.Authorization.Users.Delegation.Dto;

namespace PAX.Next.Web.Areas.Admin.Models.Layout
{
    public class ActiveUserDelegationsComboboxViewModel
    {
        public IUserDelegationConfiguration UserDelegationConfiguration { get; set; }
        
        public List<UserDelegationDto> UserDelegations { get; set; }
    }
}
