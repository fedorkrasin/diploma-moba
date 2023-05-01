using System;
using System.Collections.Generic;
using Core.Characters;
using Core.Characters.Data;
using Core.UI.Components.CharacterSelection;
using Core.UI.ViewManagement.Actors;
using UnityEngine;

namespace Core.UI.Views.Impl
{
    public class CharacterSelectionView : View, ICharacterSelectionView
    {
        [SerializeField] private Transform _selectButtonsParent;
        [SerializeField] private CharacterSelectionButton _selectButtonPrefab;

        private List<CharacterSelectionButton> _selectButtons;
        
        private void OnEnable()
        {
            _selectButtons = new List<CharacterSelectionButton>();
        }

        private void OnDisable()
        {
            foreach (var button in _selectButtons)
            {
                Destroy(button.gameObject);
            }
        }

        public void InitializeSelectButton(CharacterData data, Action<CharacterType> selectCharacter)
        {
            var button = Instantiate(_selectButtonPrefab, _selectButtonsParent);
            button.Initialize(data, () => selectCharacter.Invoke(data.Type));
            _selectButtons.Add(button);
        }
    }
}