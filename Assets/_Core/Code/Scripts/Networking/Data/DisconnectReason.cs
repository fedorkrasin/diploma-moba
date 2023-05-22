using Core.Networking.Data.Enums;
using UnityEngine;

namespace Core.Networking.Data
{
    public class DisconnectReason
    {
        public ConnectionStatus Reason { get; private set; } = ConnectionStatus.Undefined;

        public void SetDisconnectReason(ConnectionStatus reason)
        {
            Debug.Assert(reason != ConnectionStatus.Success);
            Reason = reason;
        }

        public void Clear()
        {
            Reason = ConnectionStatus.Undefined;
        }

        public bool HasTransitionReason => Reason != ConnectionStatus.Undefined;
    }
}