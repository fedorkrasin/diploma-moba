using System;
using System.Text;
using System.Threading.Tasks;
using Core.Networking.Data;
using Core.Networking.Data.Enums;
using Core.Networking.Server;
using Core.Networking.Server.Data;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Error;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using DisconnectReason = Core.Networking.Data.DisconnectReason;

namespace Core.Networking.Client
{
    public class NetworkClient : IDisposable
    {
        public event Action<ConnectionStatus> OnLocalConnection;
        public event Action<ConnectionStatus> OnLocalDisconnection;

        private const int TimeoutDuration = 10;
        private NetworkManager networkManager;
        private RelayJoinData relayJoinData;

        private DisconnectReason DisconnectReason { get; } = new DisconnectReason();

        private const string MenuSceneName = "Menu";

        public NetworkClient()
        {
            networkManager = NetworkManager.Singleton;
            networkManager.OnClientDisconnectCallback += RemoteDisconnect;
        }

        public void StartClient(string ip, int port)
        {
            var unityTransport = networkManager.gameObject.GetComponent<UnityTransport>();
            unityTransport.SetConnectionData(ip, (ushort)port);
            ConnectClient();
        }

        public async Task<JoinAllocation> StartClient(string joinCode)
        {
            JoinAllocation allocation = null;

            try
            {
                allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return null;
            }

            relayJoinData = new RelayJoinData
            {
                Key = allocation.Key,
                Port = (ushort)allocation.RelayServer.Port,
                AllocationID = allocation.AllocationId,
                AllocationIDBytes = allocation.AllocationIdBytes,
                ConnectionData = allocation.ConnectionData,
                HostConnectionData = allocation.HostConnectionData,
                IPv4Address = allocation.RelayServer.IpV4
            };

            var unityTransport = networkManager.gameObject.GetComponent<UnityTransport>();

            unityTransport.SetRelayServerData(relayJoinData.IPv4Address,
                relayJoinData.Port,
                relayJoinData.AllocationIDBytes,
                relayJoinData.Key,
                relayJoinData.ConnectionData,
                relayJoinData.HostConnectionData);

            ConnectClient();

            return allocation;
        }

        public void DisconnectClient()
        {
            DisconnectReason.SetDisconnectReason(ConnectionStatus.UserRequestedDisconnect);
            NetworkShutdown();
        }

        private void ConnectClient()
        {
            // UserData userData = ClientSingleton.Instance.Manager.User.Data;

            // string payload = JsonUtility.ToJson(userData);
            // byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

            // networkManager.NetworkConfig.ConnectionData = payloadBytes;
            networkManager.NetworkConfig.ClientConnectionBufferTimeout = TimeoutDuration;

            if (networkManager.StartClient())
            {
                Debug.Log("Starting Client!");

                RegisterListeners();
            }
            else
            {
                Debug.LogWarning("Could not start Client!");
                OnLocalDisconnection?.Invoke(ConnectionStatus.Undefined);
            }
        }

        public void RegisterListeners()
        {
            NetworkMessenger.RegisterListener(NetworkMessage.LocalClientConnected, ReceiveLocalClientConnectStatus);
            NetworkMessenger.RegisterListener(NetworkMessage.LocalClientDisconnected,
                ReceiveLocalClientDisconnectStatus);
        }

        private void ReceiveLocalClientConnectStatus(ulong clientId, FastBufferReader reader)
        {
            reader.ReadValueSafe(out ConnectionStatus status);

            Debug.Log("ReceiveLocalClientConnectStatus: " + status);

            if (status != ConnectionStatus.Success)
            {
                DisconnectReason.SetDisconnectReason(status);
            }

            OnLocalConnection?.Invoke(status);
        }

        private void ReceiveLocalClientDisconnectStatus(ulong clientId, FastBufferReader reader)
        {
            reader.ReadValueSafe(out ConnectionStatus status);
            Debug.Log("ReceiveLocalClientDisconnectStatus: " + status);
            DisconnectReason.SetDisconnectReason(status);
        }

        private void RemoteDisconnect(ulong clientId)
        {
            Debug.Log($"Got Client Disconnect callback for {clientId}");

            if (clientId != 0 && clientId != networkManager.LocalClientId)
            {
                return;
            }

            NetworkShutdown();
        }

        private void NetworkShutdown()
        {
            if (SceneManager.GetActiveScene().name != MenuSceneName)
            {
                SceneManager.LoadScene(MenuSceneName);
            }

            if (networkManager.IsConnectedClient)
            {
                networkManager.Shutdown();
            }

            OnLocalDisconnection?.Invoke(DisconnectReason.Reason);
            NetworkMessenger.UnRegisterListener(NetworkMessage.LocalClientConnected);
            NetworkMessenger.UnRegisterListener(NetworkMessage.LocalClientDisconnected);
        }

        public void Dispose()
        {
            if (networkManager != null && networkManager.CustomMessagingManager != null)
            {
                networkManager.OnClientConnectedCallback -= RemoteDisconnect;
            }
        }
    }
}