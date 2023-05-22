using System;

namespace Core.Networking.Data
{
    [Serializable]
    public class UserData
    {
        public string UserName;
        public string UserAuthId;
        public ulong ClientId;
        public GameInfo UserGamePreferences;
    
        public int CharacterId = -1;

        public UserData(string userName, string userAuthId, ulong clientId, GameInfo userGamePreferences)
        {
            UserName = userName;
            UserAuthId = userAuthId;
            ClientId = clientId;
            UserGamePreferences = userGamePreferences;
        }
    }
}