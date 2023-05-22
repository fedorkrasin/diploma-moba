using Core.Characters.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Networking.UI.CharacterSelection
{
    public class CharacterSelectionButton : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject disabledOverlay;
        [SerializeField] private Button button;

        private CharacterSelectionDisplay characterSelect;

        public CharacterData Character { get; private set; }
        public bool IsDisabled { get; private set; }

        public void SetCharacter(CharacterSelectionDisplay characterSelect, CharacterData character)
        {
            iconImage.sprite = character.Icon;

            this.characterSelect = characterSelect;
        
            Character = character;
        }

        public void SelectCharacter()
        {
            characterSelect.Select(Character);
        }

        public void SetDisabled()
        {
            IsDisabled = true;
            disabledOverlay.SetActive(true);
            button.interactable = false;
        }
    }
}