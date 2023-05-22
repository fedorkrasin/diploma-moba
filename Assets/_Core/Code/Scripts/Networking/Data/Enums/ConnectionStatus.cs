namespace Core.Networking.Data.Enums
{
    public enum ConnectionStatus
    {
        Undefined,
        Success,
        ServerFull,
        LoggedInAgain,
        UserRequestedDisconnect,
        GenericDisconnect,
        Timeout,
    }
}