using Xamarin.Forms.Internals;

namespace PAX.TaskManager.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}