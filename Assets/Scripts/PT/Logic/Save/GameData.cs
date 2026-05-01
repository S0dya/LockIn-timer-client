using PT.Logic.Configs;

namespace PT.Logic.Save
{
    public enum GameDataKey
    {
        SoundOn,
        Language,
        HighestScore,
        
        VibroOn,
        
        LevelIndex,
        
        Gold,
        
        LeaderboardPlayerRank,
        LeaderboardPlayerName,
        
        SingersOpened,
        SongsOpened,
    }
    
    public static class GameData
    {
        public static bool SoundOn { get; set; } = true;
        public static LanguageEnum Language { get; internal set; } = LanguageEnum.Ru;
        public static int HighestScore { get; internal set; }
        
        public static bool VibroOn { get; internal set; } = true;
        
        public static int LevelIndex { get; internal set; }
        
        public static int Gold { get; internal set; }
        
        public static int LeaderboardPlayerRank { get; internal set; } 
        public static string LeaderboardPlayerName { get; internal set; }

        public static string SingersOpened { get; internal set; } = "0001";
        public static string SongsOpened { get; internal set; } = "1110";
    }
}
