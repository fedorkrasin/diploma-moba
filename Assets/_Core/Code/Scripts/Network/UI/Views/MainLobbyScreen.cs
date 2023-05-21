using System;
using System.Collections.Generic;
using System.Linq;
using Core.Network.Services;
using Core.Network.UI.Components;
using Core.UI.ViewManagement.Actors;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Network.UI.Views
{
    public class MainLobbyScreen : View
    {
        [SerializeField] private LobbyRoomPanel _lobbyPanelPrefab;
        [SerializeField] private Transform _lobbyParent;
        [SerializeField] private GameObject _noLobbiesText;
        [SerializeField] private float _lobbyRefreshRate = 2;
        [SerializeField] private Button _createLobbyButton;
        
        private readonly List<LobbyRoomPanel> _currentLobbySpawns = new();
        private float _nextRefreshTime;
        
        public event Action<Lobby> LobbySelected = delegate { };
        
        public event Action CreateLobbyClicked = delegate { };

        private void Update()
        {
            if (Time.time >= _nextRefreshTime) FetchLobbies();
        }

        private void OnEnable()
        {
            foreach (Transform child in _lobbyParent) Destroy(child.gameObject);
            _currentLobbySpawns.Clear();
            
            _createLobbyButton.onClick.AddListener(OnCreateLobbyClicked);
        }

        private void OnDisable()
        {
            _createLobbyButton.onClick.RemoveListener(OnCreateLobbyClicked);
        }

        private async void FetchLobbies()
        {
            try
            {
                _nextRefreshTime = Time.time + _lobbyRefreshRate;
                
                var allLobbies = await MatchmakingService.GatherLobbies();
                var lobbyIds = allLobbies.Where(l => l.HostId != AuthenticationService.PlayerId).Select(l => l.Id);
                var notActive = _currentLobbySpawns.Where(l => !lobbyIds.Contains(l.Lobby.Id)).ToList();

                foreach (var panel in notActive)
                {
                    Destroy(panel.gameObject);
                    _currentLobbySpawns.Remove(panel);
                }

                foreach (var lobby in allLobbies)
                {
                    var current = _currentLobbySpawns.FirstOrDefault(p => p.Lobby.Id == lobby.Id);
                    if (current != null)
                    {
                        current.UpdateVisuals(lobby);
                    }
                    else
                    {
                        var panel = Instantiate(_lobbyPanelPrefab, _lobbyParent);
                        panel.Init(lobby, OnLobbySelected);
                        _currentLobbySpawns.Add(panel);
                    }
                }

                _noLobbiesText.SetActive(!_currentLobbySpawns.Any());
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void OnCreateLobbyClicked()
        {
            CreateLobbyClicked();
        }
        
        private void OnLobbySelected(Lobby lobby)
        {
            LobbySelected(lobby);
        }
    }
}