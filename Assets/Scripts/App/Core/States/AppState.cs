using App.Backend.Models;
using App.Features.Settings;
using UniRx;

namespace App.Core.States
{
    public class AppState
    {
        public ReactiveProperty<UserState> CurrentUser = new(); 
        public ReactiveProperty<RunState> RunState = new(); 
        public ReactiveProperty<TimerSettingsState> TimerSettingsState = new(); 
        
        public ReactiveProperty<int> CurrentSessionTime = new(); 
    }
}