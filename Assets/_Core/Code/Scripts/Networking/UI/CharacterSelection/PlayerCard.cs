using Core.Characters.Data;
using Core.Networking.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Networking.UI.CharacterSelection
{
    public class PlayerCard : MonoBehaviour
    {
        [SerializeField] private CharactersList _charactersList;
        [SerializeField] private GameObject _visuals;
        [SerializeField] private Image _characterIconImage;
        [SerializeField] private TMP_Text _playerNameText;
        [SerializeField] private TMP_Text _characterNameText;

        public void UpdateVisuals(CharacterSelectionState state)
        {
            if (state.CharacterId != -1)
            {
                var character = _charactersList.Get(state.CharacterId);
                _characterIconImage.sprite = character.Icon;
                _characterIconImage.enabled = true;
                _characterNameText.text = character.Name;
            }
            else
            {
                _characterIconImage.enabled = false;
            }

            _playerNameText.text = state.IsLockedIn ? $"Player {state.ClientId}" : $"Player {state.ClientId} (Picking...)";

            _visuals.SetActive(true);
        }

        public void DisableDisplay()
        {
            _visuals.SetActive(false);
        }
    }
}