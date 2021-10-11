using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using PAX.TaskManager.MultiTenancy.Accounting.Dto;

namespace PAX.TaskManager.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
