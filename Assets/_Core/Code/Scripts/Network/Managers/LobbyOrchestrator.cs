using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Network.Data;
using Core.Network.Services;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Core.Network.Managers
{
    public class LobbyOrchestrator : NetworkBehaviour
    {
        private readonly Dictionary<ulong, bool> _playersInLobby = new();
        
        private float _nextLobbyUpdate;
        public event Action<Dictionary<ulong, bool>> LobbyPlayersUpdated = delegate { };

        private AuthenticationManager _authenticationManager;

        [Inject]
        public void Construct(AuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager ?? throw new ArgumentNullException(nameof(authenticationManager));
        }
        
        private void OnEnable()
        {
            _authenticationManager.LobbyOrchestrator = this;
            NetworkObject.DestroyWithScene = true;
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
                _playersInLobby.Add(NetworkManager.Singleton.LocalClientId, false);
                OnLobbyPlayersUpdated();
            }
        
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
        }
        
        public override void OnDestroy()
        {
            base.OnDestroy();
            
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
            }
        }
        
        public async Task<bool> CreateLobby(LobbyData data)
        {
            // using (new Load("Creating Lobby..."))
            {
                try
                {
                    await MatchmakingService.CreateLobbyWithAllocation(data);
                    NetworkManager.Singleton.StartHost();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return false;
                }
            }
        }
        
        public async Task<bool> SelectLobby(Lobby lobby)
        {
            // using (new Load("Joining Lobby..."))
            {
                try
                {
                    await MatchmakingService.JoinLobbyWithAllocation(lobby.Id);
                    NetworkManager.Singleton.StartClient();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return false;
                }
            }
        }
        
        public async void LeftLobby()
        {
            // using (new Load("Leaving Lobby..."))
            {
                _playersInLobby.Clear();
                NetworkManager.Singleton.Shutdown();
                await MatchmakingService.LeaveLobby();
            }
        }
        
        public void SetReady()
        {
            SetReadyServerRpc(NetworkManager.Singleton.LocalClientId);
        }

        public async void StartGame()
        {
            // using (new Load("Starting the game..."))
            {
                await MatchmakingService.LockLobby();
                NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
            }
        }

        [ClientRpc]
        private void UpdatePlayerClientRpc(ulong clientId, bool isReady)
        {
            if (IsServer) return;

            if (!_playersInLobby.ContainsKey(clientId)) _playersInLobby.Add(clientId, isReady);
            else _playersInLobby[clientId] = isReady;
            OnLobbyPlayersUpdated();
        }

        private void PropagateToClients()
        {
            foreach (var player in _playersInLobby)
            {
                UpdatePlayerClientRpc(player.Key, player.Value);
            }
        }
        
        private void OnClientConnectedCallback(ulong playerId)
        {
            if (!IsServer) return;

            _playersInLobby.TryAdd(playerId, false);
            PropagateToClients();
            OnLobbyPlayersUpdated();
        }

        private void OnClientDisconnectCallback(ulong playerId)
        {
            if (IsServer)
            {
                if (_playersInLobby.ContainsKey(playerId)) _playersInLobby.Remove(playerId);
                RemovePlayerClientRpc(playerId);
                OnLobbyPlayersUpdated();
            }
            else
            {
                LeftLobby();
            }
        }

        [ClientRpc]
        private void RemovePlayerClientRpc(ulong clientId)
        {
            if (IsServer) return;

            if (_playersInLobby.ContainsKey(clientId)) _playersInLobby.Remove(clientId);
            OnLobbyPlayersUpdated();
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void SetReadyServerRpc(ulong playerId)
        {
            _playersInLobby[playerId] = true;
            PropagateToClients();
            OnLobbyPlayersUpdated();
        }

        private void OnLobbyPlayersUpdated()
        {
            LobbyPlayersUpdated(_playersInLobby);
        }
    }
}