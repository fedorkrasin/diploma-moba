using System;
using System.Collections.Generic;
using System.Linq;
using Core.Network.Managers;
using Core.Network.Services;
using Core.Network.UI.Components;
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
        [SerializeField] private Transform _playerPanelParent;
        [SerializeField] private TMP_Text _waitingText;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _readyButton;
        [SerializeField] private Button _exitButton;

        private bool _allReady;
        private bool _ready;

        public event Action StartPressed = delegate { };
        public event Action ReadyClicked = delegate { };
        public event Action LobbyLeft = delegate { };

        private void OnEnable()
        {
            foreach (Transform child in _playerPanelParent) Destroy(child.gameObject);
            _playerPanels.Clear();

            // LobbyOrchestrator.LobbyPlayersUpdated += NetworkLobbyPlayersUpdated;
            MatchmakingService.CurrentLobbyRefreshed += OnCurrentLobbyRefreshed;
            _startButton.gameObject.SetActive(false);
            _readyButton.gameObject.SetActive(false);

            _ready = false;
            
            _startButton.onClick.AddListener(OnStartClicked);
            _readyButton.onClick.AddListener(OnReadyClicked);
            _exitButton.onClick.AddListener(OnLeaveLobby);
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveListener(OnLeaveLobby);
            _readyButton.onClick.RemoveListener(OnReadyClicked);
            _startButton.onClick.RemoveListener(OnStartClicked);
            
            // LobbyOrchestrator.LobbyPlayersUpdated -= NetworkLobbyPlayersUpdated;
            MatchmakingService.CurrentLobbyRefreshed -= OnCurrentLobbyRefreshed;
        }

        public void NetworkLobbyPlayersUpdated(Dictionary<ulong, bool> players)
        {
            var allActivePlayerIds = players.Keys;

            // Remove all inactive panels
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
                    if (player.Value) currentPanel.SetReady();
                }
                else
                {
                    var panel = Instantiate(_playerPanelPrefab, _playerPanelParent);
                    panel.Init(player.Key);
                    _playerPanels.Add(panel);
                }
            }

            _startButton.gameObject.SetActive(NetworkManager.Singleton.IsHost && players.All(p => p.Value));
            _readyButton.gameObject.SetActive(!_ready);
        }

        private void OnCurrentLobbyRefreshed(Lobby lobby)
        {
            _waitingText.text = $"Waiting on players... {lobby.Players.Count}/{lobby.MaxPlayers}";
        }

        private void OnReadyClicked()
        {
            _readyButton.gameObject.SetActive(false);
            _ready = true;
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