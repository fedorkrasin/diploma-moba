using Core.Network.Player;
using Core.Network.Services;
using Unity.Netcode;
using UnityEngine;

namespace Core.Network.Managers
{
    public class GameManager : NetworkBehaviour
    {
        [SerializeField] private PlayerController _playerPrefab;

        public override void OnNetworkSpawn()
        {
            SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnPlayerServerRpc(ulong playerId)
        {
            var spawn = Instantiate(_playerPrefab);
            spawn.NetworkObject.SpawnWithOwnership(playerId);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            MatchmakingService.LeaveLobby();
            if (NetworkManager.Singleton != null) NetworkManager.Singleton.Shutdown();
        }
    }
}