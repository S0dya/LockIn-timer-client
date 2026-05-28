using App.Backend.Config;
using App.Backend.Models;
using Cysharp.Threading.Tasks;
using PT.Tools.Http;
using System.Collections.Generic;
using System.Threading;
using Zenject;

namespace App.Backend.Api
{
    public class RunApi : IRunApi
    {
        [Inject] private readonly IHttpClient _http;
        [Inject] private readonly ApiConfig _apiConfig;

        public UniTask<SessionStartResponse> StartSession(CancellationToken cancellationToken = default)
            => _http.Post<SessionStartResponse>(_apiConfig.StartSession, cancellationToken: cancellationToken);

        public UniTask<SessionFinishedResponse> FinishSession(CancellationToken cancellationToken = default)
            => _http.Post<SessionFinishedResponse>(_apiConfig.FinishSession, cancellationToken: cancellationToken);

        public UniTask<CancelSessionResponse> CancelSession(CancellationToken cancellationToken = default)
            => _http.Post<CancelSessionResponse>(_apiConfig.CancelSession, cancellationToken: cancellationToken);

        public UniTask<RunFinishResponse> FinishRun(RunFinishRequest request, CancellationToken cancellationToken = default)
            => _http.Post<RunFinishResponse>(_apiConfig.FinishRun, request, cancellationToken);

        public UniTask<CancelRunResponse> CancelRun(CancellationToken cancellationToken = default)
            => _http.Post<CancelRunResponse>(_apiConfig.CancelRun, cancellationToken: cancellationToken);

        public UniTask<CurrentRunResponse> GetCurrentRun(CancellationToken cancellationToken = default)
            => _http.Get<CurrentRunResponse>(_apiConfig.CurrentRun, cancellationToken);

        public UniTask<List<RunHistoryResponse>> GetRunHistory(RunHistoryRequest request, CancellationToken cancellationToken = default)
        {
            var queryString = $"?limit={request.Limit}&offset={request.Offset}";
            return _http.Get<List<RunHistoryResponse>>(_apiConfig.RunHistory + queryString, cancellationToken);
        }
    }
}
