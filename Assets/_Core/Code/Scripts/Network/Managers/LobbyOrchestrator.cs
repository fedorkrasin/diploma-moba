using System;
using System.Collections.Generic;
using Core.Network.Data;
using Core.Network.Misc;
using Core.Network.Services;
using Core.Network.Views;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Network.Managers
{
    public class LobbyOrchestrator : NetworkBehaviour
    {
        [SerializeField] private MainLobbyView _mainLobbyScreen;
        [SerializeField] private CreateLobbyView _createScreen;
        [SerializeField] private RoomView _roomScreen;

        private void OnEnable()
        {
            _mainLobbyScreen.gameObject.SetActive(true);
            _createScreen.gameObject.SetActive(false);
            _roomScreen.gameObject.SetActive(false);

            CreateLobbyView.LobbyCreated += CreateLobby;
            LobbyRoomPanel.LobbySelected += OnLobbySelected;
            RoomView.LobbyLeft += OnLobbyLeft;
            RoomView.StartPressed += OnGameStart;

            NetworkObject.DestroyWithScene = true;
        }
        
        public void OnDisable()
        {
            base.OnDestroy();
            
            RoomView.StartPressed -= OnGameStart;
            RoomView.LobbyLeft -= OnLobbyLeft;
            LobbyRoomPanel.LobbySelected -= OnLobbySelected;
            CreateLobbyView.LobbyCreated -= CreateLobby;

            // We only care about this during lobby
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
            }

        }

        #region Main Lobby

        private async void OnLobbySelected(Lobby lobby)
        {
            using (new Load("Joining Lobby..."))
            {
                try
                {
                    await MatchmakingService.JoinLobbyWithAllocation(lobby.Id);

                    _mainLobbyScreen.gameObject.SetActive(false);
                    _roomScreen.gameObject.SetActive(true);

                    NetworkManager.Singleton.StartClient();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    CanvasUtilities.Instance.ShowError("Failed joining lobby");
                }
            }
        }



        #endregion

        #region Create

        private async void CreateLobby(LobbyData data)
        {
            using (new Load("Creating Lobby..."))
            {
                try
                {
                    await MatchmakingService.CreateLobbyWithAllocation(data);

                    _createScreen.gameObject.SetActive(false);
                    _roomScreen.gameObject.SetActive(true);

                    // Starting the host immediately will keep the relay server alive
                    NetworkManager.Singleton.StartHost();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    CanvasUtilities.Instance.ShowError("Failed creating lobby");
                }
            }
        }

        #endregion

        #region Room

        private readonly Dictionary<ulong, bool> _playersInLobby = new();
        public static event Action<Dictionary<ulong, bool>> LobbyPlayersUpdated;
        private float _nextLobbyUpdate;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
                _playersInLobby.Add(NetworkManager.Singleton.LocalClientId, false);
                UpdateInterface();
            }

            // Client uses this in case host destroys the lobby
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;


        }

        private void OnClientConnectedCallback(ulong playerId)
        {
            if (!IsServer) return;

            // Add locally
            if (!_playersInLobby.ContainsKey(playerId)) _playersInLobby.Add(playerId, false);

            PropagateToClients();

            UpdateInterface();
        }

        private void PropagateToClients()
        {
            foreach (var player in _playersInLobby) UpdatePlayerClientRpc(player.Key, player.Value);
        }

        [ClientRpc]
        private void UpdatePlayerClientRpc(ulong clientId, bool isReady)
        {
            if (IsServer) return;

            if (!_playersInLobby.ContainsKey(clientId)) _playersInLobby.Add(clientId, isReady);
            else _playersInLobby[clientId] = isReady;
            UpdateInterface();
        }

        private void OnClientDisconnectCallback(ulong playerId)
        {
            if (IsServer)
            {
                // Handle locally
                if (_playersInLobby.ContainsKey(playerId)) _playersInLobby.Remove(playerId);

                // Propagate all clients
                RemovePlayerClientRpc(playerId);

                UpdateInterface();
            }
            else
            {
                // This happens when the host disconnects the lobby
                _roomScreen.gameObject.SetActive(false);
                _mainLobbyScreen.gameObject.SetActive(true);
                OnLobbyLeft();
            }
        }

        [ClientRpc]
        private void RemovePlayerClientRpc(ulong clientId)
        {
            if (IsServer) return;

            if (_playersInLobby.ContainsKey(clientId)) _playersInLobby.Remove(clientId);
            UpdateInterface();
        }

        public void OnReadyClicked()
        {
            SetReadyServerRpc(NetworkManager.Singleton.LocalClientId);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetReadyServerRpc(ulong playerId)
        {
            _playersInLobby[playerId] = true;
            PropagateToClients();
            UpdateInterface();
        }

        private void UpdateInterface()
        {
            LobbyPlayersUpdated?.Invoke(_playersInLobby);
        }

        private async void OnLobbyLeft()
        {
            using (new Load("Leaving Lobby..."))
            {
                _playersInLobby.Clear();
                NetworkManager.Singleton.Shutdown();
                await MatchmakingService.LeaveLobby();
            }
        }

        private async void OnGameStart()
        {
            using (new Load("Starting the game..."))
            {
                await MatchmakingService.LockLobby();
                NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
            }
        }
        #endregion
    }
}