using Core.UI.ViewManagement.Actors;
using Core.UI.Views;

namespace Core.UI.Presenters
{
    public class PlayerControllerPresenter : Presenter<IPlayerControllerView>
    {
        protected PlayerControllerPresenter(IPlayerControllerView view) : base(view)
        {
        }
    }
}