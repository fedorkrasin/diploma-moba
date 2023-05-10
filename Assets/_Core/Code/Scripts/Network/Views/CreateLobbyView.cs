using System;
using System.Collections.Generic;
using System.Linq;
using Core.Network.Data;
using TMPro;
using UnityEngine;

namespace Core.Network.Views
{
    public class CreateLobbyView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private TMP_InputField _maxPlayersInput;
        [SerializeField] private TMP_Dropdown _typeDropdown;
        [SerializeField] private TMP_Dropdown _difficultyDropdown;
        
        public static event Action<LobbyData> LobbyCreated = delegate { };

        private void Start()
        {
            SetOptions(_typeDropdown, Constants.GameTypes);
            SetOptions(_difficultyDropdown, Constants.Difficulties);

            void SetOptions(TMP_Dropdown dropdown, IEnumerable<string> values)
            {
                dropdown.options = values.Select(type => new TMP_Dropdown.OptionData { text = type }).ToList();
            }
        }

        public void OnCreateClicked()
        {
            var lobbyData = new LobbyData
            {
                Name = _nameInput.text,
                MaxPlayers = int.Parse(_maxPlayersInput.text),
                Difficulty = _difficultyDropdown.value,
                Type = _typeDropdown.value
            };

            LobbyCreated(lobbyData);
        }
    }
}