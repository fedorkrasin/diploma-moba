using System;
using System.Collections.Generic;
using System.Text;
using Core.Networking.Data.Enums;
using UnityEngine;

namespace Core.Networking.Data
{
    [Serializable]
    public class GameInfo
    {
        private const string MultiplayerCasualQueue = "casual-queue";
        private const string MultiplayerCompetitiveQueue = "competitive-queue";

        private static readonly Dictionary<string, GameQueue> MultiplayerToLocalQueueNames = new()
        {
            { MultiplayerCasualQueue, GameQueue.Casual },
            { MultiplayerCompetitiveQueue, GameQueue.Competitive }
        };

        public Map Map;
        public GameMode GameMode;
        public GameQueue GameQueue;

        public int MaxUsers = 20;
        public string ToSceneName => ConvertToScene(Map);

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("GameInfo: ");
            sb.AppendLine($"- map:        {Map}");
            sb.AppendLine($"- gameMode:   {GameMode}");
            sb.AppendLine($"- gameQueue:  {GameQueue}");
            return sb.ToString();
        }

        public static string ConvertToScene(Map map)
        {
            switch (map)
            {
                case Map.Default:
                    return "Gameplay";
                default:
                    Debug.LogWarning($"{map} - is not supported.");
                    return "";
            }
        }

        public string ToMultiplayerQueue()
        {
            return GameQueue switch
            {
                GameQueue.Casual => MultiplayerCasualQueue,
                GameQueue.Competitive => MultiplayerCompetitiveQueue,
                _ => MultiplayerCasualQueue
            };
        }

        public static GameQueue ToGameQueue(string multiplayQueue)
        {
            if (!MultiplayerToLocalQueueNames.ContainsKey(multiplayQueue))
            {
                Debug.LogWarning($"No QueuePreference that maps to {multiplayQueue}");
                return GameQueue.Casual;
            }

            return MultiplayerToLocalQueueNames[multiplayQueue];
        }
    }
}