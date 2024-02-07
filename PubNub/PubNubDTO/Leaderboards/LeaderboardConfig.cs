using System;

namespace Pubnub.DTO
{
    [Serializable]
    public class LeaderboardConfig
    {
        public int PositionsToDisplay = 100;
        public string LeaderboardID = string.Empty;
        public string LeaderboardVersion = string.Empty;
    }
}
