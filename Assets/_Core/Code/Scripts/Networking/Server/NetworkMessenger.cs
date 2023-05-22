﻿using Core.Networking.Server.Data;
using Unity.Netcode;

namespace Core.Networking.Server
{
    public static class NetworkMessenger
    {
        public static void SendMessageToAll(NetworkMessage messageType, FastBufferWriter writer)
        {
            NetworkManager.Singleton.CustomMessagingManager.SendNamedMessageToAll(messageType.ToString(), writer);
        }

        public static void SendMessageTo(NetworkMessage messageType, ulong clientId, FastBufferWriter writer)
        {
            NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(messageType.ToString(), clientId, writer);
        }

        public static void RegisterListener(NetworkMessage messageType, CustomMessagingManager.HandleNamedMessageDelegate listenerMethod)
        {
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(messageType.ToString(), listenerMethod);
        }

        public static void UnRegisterListener(NetworkMessage messageType)
        {
            if (NetworkManager.Singleton == null) return;
            NetworkManager.Singleton.CustomMessagingManager.UnregisterNamedMessageHandler(messageType.ToString());
        }
    }
}