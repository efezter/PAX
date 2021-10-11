using System.Threading.Tasks;
using Abp.Dependency;

namespace PAX.TaskManager.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}