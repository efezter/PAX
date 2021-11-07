using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using PAX.Next.MultiTenancy.Accounting.Dto;

namespace PAX.Next.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
