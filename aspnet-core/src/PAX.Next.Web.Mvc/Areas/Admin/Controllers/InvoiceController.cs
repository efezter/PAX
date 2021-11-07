using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using PAX.Next.MultiTenancy.Accounting;
using PAX.Next.Web.Areas.Admin.Models.Accounting;
using PAX.Next.Web.Controllers;

namespace PAX.Next.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InvoiceController : NextControllerBase
    {
        private readonly IInvoiceAppService _invoiceAppService;

        public InvoiceController(IInvoiceAppService invoiceAppService)
        {
            _invoiceAppService = invoiceAppService;
        }


        [HttpGet]
        public async Task<ActionResult> Index(long paymentId)
        {
            var invoice = await _invoiceAppService.GetInvoiceInfo(new EntityDto<long>(paymentId));
            var model = new InvoiceViewModel
            {
                Invoice = invoice
            };

            return View(model);
        }
    }
}