using System;
using Core.Networking.Data.Enums;
using Unity.Collections;
using Unity.Netcode;

namespace Core.Networking.Data
{
    public class ServerData : NetworkBehaviour
    {
        public NetworkVariable<FixedString32Bytes> ServerId = new();
        public NetworkVariable<Map> Map = new();
        public NetworkVariable<GameMode> GameMode = new();
        public NetworkVariable<GameQueue> GameQueue = new();

        public Action NetworkSpawned = delegate { };

        public override void OnNetworkSpawn()
        {
            NetworkSpawned();
        }
    }
}