using UniRx;

namespace App.Timer.States
{
    public class InternetState
    {
        // public ReactiveProperty<bool> IsInitialized = new();
        public ReactiveProperty<bool> IsConnected = new();
    }
}