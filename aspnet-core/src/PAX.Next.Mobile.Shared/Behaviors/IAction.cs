using Xamarin.Forms.Internals;

namespace PAX.Next.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}