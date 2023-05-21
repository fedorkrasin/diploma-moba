using System;
using System.Collections.Generic;
using System.Linq;
using Core.Network.Data;
using Core.UI.ViewManagement.Actors;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Network.UI.Views
{
    public class CreateLobbyScreen : View
    {
        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private TMP_InputField _maxPlayersInput;
        [SerializeField] private TMP_Dropdown _typeDropdown;
        [SerializeField] private Button _createButton;
        
        public event Action<LobbyData> LobbyCreated = delegate { };

        private void OnEnable()
        {
            _createButton.onClick.AddListener(OnCreateClicked);
        }
        
        private void OnDisable()
        {
            _createButton.onClick.RemoveListener(OnCreateClicked);
        }

        private void Start()
        {
            SetOptions(_typeDropdown, Constants.GameTypes);

            void SetOptions(TMP_Dropdown dropdown, IEnumerable<string> values)
            {
                dropdown.options = values.Select(type => new TMP_Dropdown.OptionData { text = type }).ToList();
            }
        }

        private void OnCreateClicked()
        {
            var lobbyData = new LobbyData
            {
                Name = _nameInput.text,
                MaxPlayers = int.Parse(_maxPlayersInput.text),
                Type = _typeDropdown.value
            };

            LobbyCreated(lobbyData);
        }

        
    }
}