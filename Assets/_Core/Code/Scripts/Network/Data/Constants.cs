﻿using System.Collections.Generic;

namespace Core.Network.Data
{
    public class Constants
    {
        public const string JoinKey = "j";
        public const string DifficultyKey = "d";
        public const string GameTypeKey = "t";

        public static readonly List<string> GameTypes = new() { "Battle Royal", "Capture The Flag", "Creative" };
        public static readonly List<string> Difficulties = new() { "Easy", "Medium", "Hard" };
    }
}