using System;
using System.Text;
using Core.Networking.Data.Enums;

namespace Core.Networking.Data
{
    public class MatchplayUser
    {
        public UserData Data { get; }

        public event Action<string> NameChanged = delegate {  };

        public MatchplayUser()
        {
            var tempId = Guid.NewGuid().ToString();

            Data = new UserData(
                "Player",
                tempId,
                0,
                new GameInfo());
        }

        public string Name
        {
            get => Data.UserName;
            set
            {
                Data.UserName = value;
                OnNameChanged(Data.UserName);
            }
        }

        public string AuthId
        {
            get => Data.UserAuthId;
            set => Data.UserAuthId = value;
        }

        public Map MapPreferences
        {
            get => Data.UserGamePreferences.Map;
            set => Data.UserGamePreferences.Map = value;
        }

        public GameMode GameModePreferences
        {
            get => Data.UserGamePreferences.GameMode;
            set => Data.UserGamePreferences.GameMode = value;
        }

        public GameQueue QueuePreference
        {
            get => Data.UserGamePreferences.GameQueue;
            set => Data.UserGamePreferences.GameQueue = value;
        }

        public override string ToString()
        {
            var userData = new StringBuilder("MatchplayUser: ");
            userData.AppendLine($"- {Data}");
            return userData.ToString();
        }

        private void OnNameChanged(string name)
        {
            NameChanged(name);
        }
    }
}