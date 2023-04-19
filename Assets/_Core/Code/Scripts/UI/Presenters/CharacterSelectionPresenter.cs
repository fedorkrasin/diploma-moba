using System;
using Core.Bootstrap.Commands;
using Core.Characters.Data;
using Core.UI.ViewManagement.Actors;
using Core.UI.Views;

namespace Core.UI.Presenters
{
    public class CharacterSelectionPresenter : Presenter<ICharacterSelectionView>
    {
        private readonly CharactersList _charactersList;
        private readonly ISelectCharacterCommand _selectCharacterCommand;
        
        protected CharacterSelectionPresenter(
            CharactersList charactersList,
            ISelectCharacterCommand selectCharacterCommand,
            ICharacterSelectionView view) : base(view)
        {
            _charactersList = charactersList ? charactersList : throw new ArgumentNullException(nameof(charactersList));
            _selectCharacterCommand = selectCharacterCommand ?? throw new ArgumentNullException(nameof(selectCharacterCommand));
        }

        public override void Initialize()
        {
            InitializeSelectButtons();
        }

        public override void Dispose()
        {
            
        }

        private void InitializeSelectButtons()
        {
            foreach (var data in _charactersList.CharactersData)
            {
                View.InitializeSelectButton(data, _selectCharacterCommand.Execute);
            }
        }
    }
}