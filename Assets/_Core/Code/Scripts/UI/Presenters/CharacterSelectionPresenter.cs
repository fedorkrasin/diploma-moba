using Core.UI.ViewManagement.Actors;
using Core.UI.Views;

namespace Core.UI.Presenters
{
    public class CharacterSelectionPresenter : Presenter<ICharacterSelectionView>
    {
        protected CharacterSelectionPresenter(ICharacterSelectionView view) : base(view)
        {
        }
    }
}