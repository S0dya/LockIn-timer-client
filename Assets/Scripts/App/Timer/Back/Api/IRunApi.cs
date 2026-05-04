using App.Timer.Back.Models;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace App.Timer.Back.Api
{
    public interface IRunApi
    {
        UniTask<SessionStartResponse> StartSession(CancellationToken cancellationToken = default);
        UniTask<SessionFinishedResponse> FinishSession(CancellationToken cancellationToken = default);
        UniTask CancelSession(CancellationToken cancellationToken = default);
        UniTask<RunFinishResponse> FinishRun(RunFinishRequest request, CancellationToken cancellationToken = default);
        UniTask CancelRun(CancellationToken cancellationToken = default);
        UniTask<CurrentRunResponse> GetCurrentRun(CancellationToken cancellationToken = default);
        UniTask<List<RunHistoryResponse>> GetRunHistory(RunHistoryRequest request, CancellationToken cancellationToken = default);
    }
}
