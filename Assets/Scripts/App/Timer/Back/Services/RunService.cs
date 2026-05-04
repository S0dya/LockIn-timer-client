using App.Timer.Back.Api;
using App.Timer.Back.Models;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Zenject;

namespace App.Timer.Back.Services
{
    public class RunService
    {
        [Inject] private readonly IRunApi _api;

        public UniTask<SessionStartResponse> StartSession(CancellationToken cancellationToken = default)
            => _api.StartSession(cancellationToken);

        public UniTask<SessionFinishedResponse> FinishSession(CancellationToken cancellationToken = default)
            => _api.FinishSession(cancellationToken);

        public UniTask CancelSession(CancellationToken cancellationToken = default)
            => _api.CancelSession(cancellationToken);

        public UniTask<RunFinishResponse> FinishRun(RunFinishRequest request, CancellationToken cancellationToken = default)
            => _api.FinishRun(request, cancellationToken);

        public UniTask CancelRun(CancellationToken cancellationToken = default)
            => _api.CancelRun(cancellationToken);

        public UniTask<CurrentRunResponse> GetCurrentRun(CancellationToken cancellationToken = default)
            => _api.GetCurrentRun(cancellationToken);

        public UniTask<List<RunHistoryResponse>> GetRunHistory(RunHistoryRequest request, CancellationToken cancellationToken = default)
            => _api.GetRunHistory(request, cancellationToken);
    }
}
