using Unity.Netcode;

namespace Core.Network.Data
{
    public class LobbyPlayer : INetworkSerializable
    {
        public bool IsReady;
        public int SelectedCharacterId;

        public LobbyPlayer()
        {
            IsReady = false;
            SelectedCharacterId = 0;
        }

        public LobbyPlayer(bool isReady, int selectedCharacterId)
        {
            IsReady = isReady;
            SelectedCharacterId = selectedCharacterId;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref IsReady);
            serializer.SerializeValue(ref SelectedCharacterId);
        }
    }
}