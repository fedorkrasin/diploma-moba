using Core.Networking.Data.Enums;

namespace Core.Networking.Data
{
    public class MatchmakingResult
    {
        public string Ip;
        public int Port;
        public MatchmakerPollingResult Result;
        public string ResultMessage;
    }
}