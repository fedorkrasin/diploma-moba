using System.Collections.Generic;

namespace Core.Network.Data
{
    public class Constants
    {
        public const string JoinKey = "j";
        public const string DifficultyKey = "d";
        public const string GameTypeKey = "t";
        public const string MapKey = "m";

        public static readonly List<string> GameTypes = new() { "Solo Showdown", "Double Showdown" };
        public static readonly List<string> Maps = new() { "Forest", "Desert" };
    }
}