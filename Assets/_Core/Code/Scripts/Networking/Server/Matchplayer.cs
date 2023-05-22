using System;
using Unity.Netcode;

namespace Core.Networking.Server
{
    public class Matchplayer : NetworkBehaviour
    {
        public static event Action<Matchplayer> ServerPlayerSpawned = delegate { };
        public static event Action<Matchplayer> ServerPlayerDespawned = delegate { };

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                ServerPlayerSpawned(this);
            }

            if (IsClient)
            {
                // ClientSingleton.Instance.Manager.AddMatchPlayer(this);
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsServer)
            {
                ServerPlayerDespawned(this);
            }

            if (IsClient)
            {
                // ClientSingleton.Instance.Manager.RemoveMatchPlayer(this);
            }
        }
    }
}