#if UNITY_IOS || UNITY_ANDROID
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PT.Backend.Types;
using PT.Tools.Leaderboard;
using Zenject;

namespace PT.Backend.GP
{
    public class GooglePlayLeaderboardService : ILeaderboardService
    {
        [Inject] private LeaderboardConfig _leaderboardConfig;
        
        public async UniTask SetScore(long score)
        {
            /*
            Google Play Leaderboard SetScore implementation is intentionally disabled.
            */
            await UniTask.CompletedTask;
        }
        
        public async UniTask<LeaderboardSnapshot> GetTop(int count)
        {
            /*
            Google Play Leaderboard GetTop implementation is intentionally disabled.
            */
            var entries = new List<LeaderboardEntry>();
            var playerEntry = new LeaderboardEntry();
            await UniTask.CompletedTask;
            return new LeaderboardSnapshot(playerEntry, entries);
        }

        public async UniTask<LeaderboardSnapshot> GetAroundPlayer(int range)
        {
            /*
            Google Play Leaderboard GetAroundPlayer implementation is intentionally disabled.
            */
            var entries = new List<LeaderboardEntry>();
            var playerEntry = new LeaderboardEntry();
            await UniTask.CompletedTask;
            return new LeaderboardSnapshot(playerEntry, entries);
        }
    }
}
#endif