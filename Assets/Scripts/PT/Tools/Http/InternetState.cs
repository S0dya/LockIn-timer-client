using UniRx;

namespace PT.Tools.Http
{
    public class InternetState
    {
        // public ReactiveProperty<bool> IsInitialized = new();
        public ReactiveProperty<bool> IsConnected = new();
    }
}