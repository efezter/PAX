using System.Threading.Tasks;
using Abp.Dependency;

namespace PAX.Next.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}