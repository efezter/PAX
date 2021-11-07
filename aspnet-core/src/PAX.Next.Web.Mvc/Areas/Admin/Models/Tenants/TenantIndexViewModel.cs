using System.Collections.Generic;
using PAX.Next.Editions.Dto;

namespace PAX.Next.Web.Areas.Admin.Models.Tenants
{
    public class TenantIndexViewModel
    {
        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }
    }
}