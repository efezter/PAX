using System.Collections.Generic;
using PAX.TaskManager.Editions.Dto;

namespace PAX.TaskManager.Web.Areas.Admin.Models.Tenants
{
    public class TenantIndexViewModel
    {
        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }
    }
}