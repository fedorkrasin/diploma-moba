using System;
using System.Collections.Generic;
using Core.Network.Data;
using Core.Network.Managers;
using Core.Network.UI.Views;
using Core.UI.ViewManagement.Actors;
using Unity.Services.Lobbies.Models;

namespace Core.Network.UI.Presenters
{
    public class LobbyPresenter : Presenter<LobbyView>
    {
        private readonly AuthenticationManager _authenticationManager;
        
        protected LobbyPresenter(
            AuthenticationManager authenticationManager,
            LobbyView view) : base(view)
        {
            _authenticationManager = authenticationManager ?? throw new ArgumentNullException(nameof(authenticationManager));
        }

        public override void Initialize()
        {
            _authenticationManager.LobbyOrchestrator.LobbyPlayersUpdated += View.Room.NetworkLobbyPlayersUpdated;

            View.MainLobby.CreateLobbyClicked += OnCreateLobbyClicked;
            View.CreateLobby.LobbyCreated += OnLobbyCreated;
            View.MainLobby.LobbySelected += OnLobbySelected;
            View.Room.LobbyLeft += OnLobbyLeft;
            View.Room.ReadyClicked += OnReadyClicked;
            View.Room.StartPressed += OnGameStarted;

            View.ShowMainLobby();
        }
        
        public override void Dispose()
        {
            View.Room.StartPressed -= OnGameStarted;
            View.Room.ReadyClicked -= OnReadyClicked;
            View.Room.LobbyLeft -= OnLobbyLeft;
            View.MainLobby.LobbySelected -= OnLobbySelected;
            View.CreateLobby.LobbyCreated -= OnLobbyCreated;
            View.MainLobby.CreateLobbyClicked -= OnCreateLobbyClicked;
            
            _authenticationManager.LobbyOrchestrator.LobbyPlayersUpdated -= View.Room.NetworkLobbyPlayersUpdated;
        }

        private void OnCreateLobbyClicked()
        {
            View.ShowCreateLobby();
        }

        private async void OnLobbyCreated(LobbyData lobbyData)
        {
            if (await _authenticationManager.LobbyOrchestrator.CreateLobby(lobbyData))
            {
                View.ShowRoom();
            }
            else
            {
                // CanvasUtilities.Instance.ShowError("Failed creating lobby");
            }
        }

        private async void OnLobbySelected(Lobby lobby)
        {
            if (await _authenticationManager.LobbyOrchestrator.SelectLobby(lobby))
            {
                View.ShowRoom();
            }
            else
            {
                // CanvasUtilities.Instance.ShowError("Failed joining lobby");
            }
        }

        private void OnLobbyLeft()
        {
            _authenticationManager.LobbyOrchestrator.LeftLobby();
            View.ShowMainLobby();
        }

        private void OnLobbyPlayersUpdated(Dictionary<ulong, bool> playersInLobby)
        {
            
        }

        private void OnReadyClicked()
        {
            _authenticationManager.LobbyOrchestrator.SetReady();
        }

        private void OnGameStarted()
        {
            _authenticationManager.LobbyOrchestrator.StartGame();
        }
    }
}