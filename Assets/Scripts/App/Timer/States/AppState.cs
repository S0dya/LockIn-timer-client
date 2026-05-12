using App.Timer.Back.Models;
using App.Timer.Settings;
using UniRx;

namespace App.Timer.States
{
    public class AppState
    {
        public ReactiveProperty<UserState> CurrentUser = new(); 
        public ReactiveProperty<RunState> RunState = new(); 
        public ReactiveProperty<TimerSettingsState> TimerSettingsState = new(); 
        
        public ReactiveProperty<int> CurrentSessionTime = new(); 
    }
}