using PT.Tools.Helper;
using UnityEngine;

namespace App.Timer
{
    [CreateAssetMenu(menuName = "Configs/AppConfig", fileName = "AppConfig")]
    public class AppConfig : ScriptableObject
    {
        [SerializeField] int[] timerSettingsDurations;
        [SerializeField] int[] timerSettingsSessionsAmounts;
        [Space]
        [SerializeField] int minSessionTimeThreshold = 3;
        [SerializeField] int settingsTolerance = 5;
        [Space]
        [SerializeField] int defaultTimerSettingsSessionDuration = 1500;
        [SerializeField] int defaultTimerSettingsSessionsAmount = 2;
        
        public int[] TimerSettingsDurations => timerSettingsDurations;
        public int[] TimerSettingsSessionsAmounts => timerSettingsSessionsAmounts;
        
        public int MinSessionTimeThreshold => minSessionTimeThreshold;
        public int SettingsTolerance => settingsTolerance;
        
        public int DefaultTimerSettingsSessionDuration => defaultTimerSettingsSessionDuration;
        public int DefaultTimerSettingsSessionsAmount => defaultTimerSettingsSessionsAmount;
    }
}