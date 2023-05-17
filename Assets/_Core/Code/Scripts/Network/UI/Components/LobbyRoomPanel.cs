using System;
using Core.Network.Data;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Network.UI.Components
{
    public class LobbyRoomPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _typeText;
        [SerializeField] private TMP_Text _playerCountText;
        [SerializeField] private Button _lobbySelectButton;

        public Lobby Lobby { get; private set; }

        public static event Action<Lobby> LobbySelected = delegate { };

        public void Init(Lobby lobby)
        {
            UpdateDetails(lobby);
        }

        private void OnEnable()
        {
            _lobbySelectButton.onClick.AddListener(OnLobbySelected);
        }

        private void OnDisable()
        {
            _lobbySelectButton.onClick.RemoveListener(OnLobbySelected);
        }

        public void UpdateDetails(Lobby lobby)
        {
            Lobby = lobby;
            _nameText.text = lobby.Name;
            _typeText.text = Constants.GameTypes[GetValue(Constants.GameTypeKey)];

            _playerCountText.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";

            int GetValue(string key)
            {
                return int.Parse(lobby.Data[key].Value);
            }
        }

        private void OnLobbySelected()
        {
            LobbySelected(Lobby);
        }
    }
}