using System;
using Core.Characters.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Components.CharacterSelection
{
    public class CharacterSelectionButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameLabel;
        [SerializeField] private Button _button;

        private Action _selectCharacter; 
        
        public void Initialize(CharacterData data, Action selectCharacter)
        {
            _nameLabel.text = data.Name;
            _selectCharacter = selectCharacter;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnCharacterSelected);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnCharacterSelected);
        }

        private void OnCharacterSelected()
        {
            _selectCharacter.Invoke();
        }
    }
}