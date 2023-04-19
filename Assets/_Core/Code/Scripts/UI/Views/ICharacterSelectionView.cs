using System;
using Core.Characters;
using Core.Characters.Data;
using Core.UI.ViewManagement.Actors;

namespace Core.UI.Views
{
    public interface ICharacterSelectionView : IView
    {
        void InitializeSelectButton(CharacterData data, Action<CharacterType> selectCharacter);
    }
}