using System.Threading.Tasks;
using PAX.Next.Views;
using Xamarin.Forms;

namespace PAX.Next.Services.Modal
{
    public interface IModalService
    {
        Task ShowModalAsync(Page page);

        Task ShowModalAsync<TView>(object navigationParameter) where TView : IXamarinView;

        Task<Page> CloseModalAsync();
    }
}
