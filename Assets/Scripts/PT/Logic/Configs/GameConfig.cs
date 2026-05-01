using UnityEngine;

namespace PT.Logic.Configs
{
    [CreateAssetMenu(menuName = "Configs/GameConfig", fileName = "GameConfig")]
    public class GameConfig : BaseGameConfig
    {
        [Space(20)]
        [Header("GAME settings:")]
        [SerializeField] private float lowestBallYOffset = 5;
        [SerializeField] private float gameEndDelay = 1;
        [Space]
        [SerializeField] private float vocalsRampSeconds = 0.25f;
        [SerializeField] private float songEndingBeforeFinishSeconds = 5f;
        [Space]
        [SerializeField] private int minSingersAmount = 2;
        [SerializeField] private int maxSingersAmount = 5;
        [SerializeField] private int maxTotalSingersAmount = 15;
        [Space]
        [SerializeField] private float levelChunksOpeningYOffset = 14;
        [SerializeField] private int levelChunksInitialEnabled = 3;
        [SerializeField] private int levelChunksTopDisableStep = 3;
        
        
        public float LowestBallYOffset => lowestBallYOffset;
        public float GameEndDelay => gameEndDelay;
        
        public float VocalsRampSeconds => vocalsRampSeconds;
        public float SongEndingBeforeFinishSeconds => songEndingBeforeFinishSeconds;
        
        public int MinSingersAmount => minSingersAmount;
        public int MaxSingersAmount => maxSingersAmount;
        public int MaxTotalSingersAmount => maxTotalSingersAmount;
        
        public float LevelChunksOpeningYOffset => levelChunksOpeningYOffset;
        public int LevelChunksInitialEnabled => levelChunksInitialEnabled;
        public int LevelChunksTopDisableStep => levelChunksTopDisableStep;
        
        // public Vector2Int InitialAddedElements =>
        //     new Vector2Int(
        //         RCInt(RemoteConfigKeys.InitialAddedElementsMin, initialAddedElements.x),
        //         RCInt(RemoteConfigKeys.InitialAddedElementsMax, initialAddedElements.y)
        //     );
        //
        // public bool StrictPushDirections =>
        //     RCBool(RemoteConfigKeys.StrictPushDirections, strictPushDirections);
        //
        // public float MegaMergeChargePerMerge =>
        //     RCFloat(RemoteConfigKeys.MegaMergeChargePerMerge, megaMergeChargePerMerge);
    }
}