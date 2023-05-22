using System;

namespace Core.Networking.Data
{
    [Serializable]
    public class ClientData
    {
        public ulong ClientId;
        public int CharacterId = -1;

        public ClientData(ulong clientId)
        {
            ClientId = clientId;
        }
    }
}