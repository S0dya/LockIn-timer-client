using PT.Logic.Configs;

namespace PT.Logic.Save
{
    public enum GameDataKey
    {
        SoundOn,
        Language,
        HighestScore,
        
        VibroOn,
        
        NotificationsRequested,
        NotificationsOn,
        
        LevelIndex,
        
        Gold,
        
        LeaderboardPlayerRank,
        LeaderboardPlayerName,
    }
    
    public static class GameData
    {
        public static bool SoundOn { get; set; } = true;
        public static LanguageEnum Language { get; internal set; } = LanguageEnum.Ru;
        public static int HighestScore { get; internal set; }
        
        public static bool VibroOn { get; internal set; } = true;

        public static bool NotificationsRequested { get; internal set; } = false;
        public static bool NotificationsOn { get; internal set; } = false;
        
        public static int LevelIndex { get; internal set; }
        
        public static int Gold { get; internal set; }
        
        public static int LeaderboardPlayerRank { get; internal set; } 
        public static string LeaderboardPlayerName { get; internal set; }
    }
}
