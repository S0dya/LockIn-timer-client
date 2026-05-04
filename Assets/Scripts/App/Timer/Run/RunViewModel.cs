using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

namespace App.Timer.Run
{
    public class RunViewModel
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public CancellationToken CancellationToken => _cts.Token;

        public void CancelAllRequests()
        {
            _cts.Cancel();
        }

        public void Dispose()
        {
            _cts?.Dispose();
        }
    }
}