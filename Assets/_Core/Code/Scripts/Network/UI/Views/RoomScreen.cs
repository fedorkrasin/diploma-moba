using System;
using System.Collections.Generic;
using System.Linq;
using Core.Characters;
using Core.Characters.Data;
using Core.Network.Data;
using Core.Network.Services;
using Core.Network.UI.Components;
using Core.UI.Components.CharacterSelection;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using View = Core.UI.ViewManagement.Actors.View;

namespace Core.Network.UI.Views
{
    public class RoomScreen : View
    {
        private readonly List<LobbyPlayerPanel> _playerPanels = new();
        
        [SerializeField] private LobbyPlayerPanel _playerPanelPrefab;
        [SerializeField] private CharacterSelectionButton _selectButtonPrefab;
        [SerializeField] private Transform _playerPanelParent;
        [SerializeField] private Transform _charactersParent;
        [SerializeField] private TMP_Text _waitingText;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _readyButton;
        [SerializeField] private Button _exitButton;

        private List<CharacterSelectionButton> _characterSelectionButtons = new();
        private bool _isReady;
        private bool _areAllReady;

        public event Action StartPressed = delegate { };
        public event Action ReadyClicked = delegate { };
        public event Action LobbyLeft = delegate { };

        private void OnEnable()
        {
            foreach (Transform child in _playerPanelParent)
            {
                Destroy(child.gameObject);
            }
            
            _characterSelectionButtons = new List<CharacterSelectionButton>();

            _playerPanels.Clear();
            _isReady = false;
            _startButton.gameObject.SetActive(false);
            _readyButton.gameObject.SetActive(false);

            MatchmakingService.CurrentLobbyRefreshed += OnCurrentLobbyRefreshed;

            _startButton.onClick.AddListener(OnStartClicked);
            _readyButton.onClick.AddListener(OnReadyClicked);
            _exitButton.onClick.AddListener(OnLeaveLobby);
        }

        private void OnDisable()
        {
            foreach (var button in _characterSelectionButtons)
            {
                Destroy(button.gameObject);
            }
            
            _exitButton.onClick.RemoveListener(OnLeaveLobby);
            _readyButton.onClick.RemoveListener(OnReadyClicked);
            _startButton.onClick.RemoveListener(OnStartClicked);
            
            MatchmakingService.CurrentLobbyRefreshed -= OnCurrentLobbyRefreshed;
        }
        
        public void InitializeSelectButton(CharacterData data, Action<int> selectCharacter)
        {
            var button = Instantiate(_selectButtonPrefab, _charactersParent);
            button.Initialize(data, () => selectCharacter.Invoke(data.Id));
            _characterSelectionButtons.Add(button);
        }

        public void OnLobbyPlayersUpdated(Dictionary<ulong, LobbyPlayer> players)
        {
            var allActivePlayerIds = players.Keys;

            var toDestroy = _playerPanels.Where(p => !allActivePlayerIds.Contains(p.PlayerId)).ToList();
            foreach (var panel in toDestroy)
            {
                _playerPanels.Remove(panel);
                Destroy(panel.gameObject);
            }

            foreach (var player in players)
            {
                var currentPanel = _playerPanels.FirstOrDefault(p => p.PlayerId == player.Key);
                if (currentPanel != null)
                {
                    if (player.Value.IsReady) currentPanel.SetReady();
                }
                else
                {
                    var panel = Instantiate(_playerPanelPrefab, _playerPanelParent);
                    panel.Init(player.Key);
                    _playerPanels.Add(panel);
                }
            }

            _startButton.gameObject.SetActive(NetworkManager.Singleton.IsHost && players.All(p => p.Value.IsReady));
            _readyButton.gameObject.SetActive(!_isReady);
        }

        private void OnCurrentLobbyRefreshed(Lobby lobby)
        {
            _waitingText.text = $"Waiting on players... {lobby.Players.Count}/{lobby.MaxPlayers}";
        }

        private void OnReadyClicked()
        {
            _readyButton.gameObject.SetActive(false);
            _isReady = true;
            ReadyClicked();
        }

        private void OnStartClicked()
        {
            StartPressed();
        }
        
        private void OnLeaveLobby()
        {
            LobbyLeft();
        }
    }
}