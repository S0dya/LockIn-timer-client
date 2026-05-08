using UniRx;

namespace App.Timer.Windows
{
    public class AppWindowsState
    {
        public ReactiveProperty<bool> IsLoading = new(false);
        public ReactiveProperty<string> Error = new("");
    }
}