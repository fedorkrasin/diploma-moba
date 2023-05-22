using System;
using Core.Network.Player;
using Core.Network.Services;
using Core.Player;
using Core.UI.ViewManagement;
using Core.UI.ViewManagement.Data;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Core.Network.Managers
{
    public class GameManager : NetworkBehaviour
    {
        [SerializeField] private PlayerBehaviour _playerPrefab;

        private ViewManager _viewManager;
        
        [Inject]
        private void Construct(ViewManager viewManager)
        {
            _viewManager = viewManager ?? throw new ArgumentNullException(nameof(viewManager));
        }

        public override void OnNetworkSpawn()
        {
            _viewManager.OpenView(ViewId.PlayerController);
            SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnPlayerServerRpc(ulong playerId)
        {
            var spawn = Instantiate(_playerPrefab);
            spawn.NetworkObject.SpawnWithOwnership(playerId);
        }

        public override async void OnDestroy()
        {
            base.OnDestroy();
            await MatchmakingService.LeaveLobby();
            if (NetworkManager.Singleton != null) NetworkManager.Singleton.Shutdown();
        }
    }
}