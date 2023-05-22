using System;
using System.Collections.Generic;
using Core.Bootstrap.Commands;
using Core.Characters.Data;
using Core.Network.Data;
using Core.Network.Managers;
using Core.Network.UI.Views;
using Core.UI.ViewManagement.Actors;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Core.Network.UI.Presenters
{
    public class LobbyPresenter : Presenter<LobbyView>
    {
        private readonly AuthenticationManager _authenticationManager;
        private readonly CharactersList _charactersList;
        // private readonly ISelectCharacterCommand _selectCharacterCommand;
        
        protected LobbyPresenter(
            AuthenticationManager authenticationManager,
            CharactersList charactersList,
            // ISelectCharacterCommand selectCharacterCommand,
            LobbyView view) : base(view)
        {
            _authenticationManager = authenticationManager ?? throw new ArgumentNullException(nameof(authenticationManager));
            _charactersList = charactersList ? charactersList : throw new ArgumentNullException(nameof(charactersList));
            // _selectCharacterCommand = selectCharacterCommand ?? throw new ArgumentNullException(nameof(selectCharacterCommand));
        }

        public override void Initialize()
        {
            _authenticationManager.LobbyOrchestrator.LobbyPlayersUpdated += OnLobbyPlayersUpdated;

            View.MainLobby.CreateLobbyClicked += OnCreateLobbyClicked;
            View.CreateLobby.LobbyCreated += OnLobbyCreated;
            View.MainLobby.LobbySelected += OnLobbySelected;
            View.Room.LobbyLeft += OnLobbyLeft;
            View.Room.ReadyClicked += OnReadyClicked;
            View.Room.StartPressed += OnGameStarted;

            InitializeSelectButtons();
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
            
            _authenticationManager.LobbyOrchestrator.LobbyPlayersUpdated -= OnLobbyPlayersUpdated;
        }
        
        private void InitializeSelectButtons()
        {
            foreach (var data in _charactersList.CharactersData)
            {
                View.Room.InitializeSelectButton(data, OnSelectCharacterClicked);
            }
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
                OnLobbyPlayersUpdated(_authenticationManager.PlayersInLobby);
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
                OnLobbyPlayersUpdated(_authenticationManager.PlayersInLobby);
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

        private void OnLobbyPlayersUpdated(Dictionary<ulong, LobbyPlayer> playersInLobby)
        {
            View.Room.OnLobbyPlayersUpdated(playersInLobby);
        }

        private void OnReadyClicked()
        {
            _authenticationManager.LobbyOrchestrator.SetReady();
        }

        private void OnSelectCharacterClicked(int characterId)
        {
            _authenticationManager.LobbyOrchestrator.SelectCharacter(characterId);
        }

        private void OnGameStarted()
        {
            _authenticationManager.LobbyOrchestrator.StartGame();
        }
    }
}