using App.Timer.Back.Api;
using App.Timer.Back.Models;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using PT.Tools.Http;
using Zenject;

namespace App.Timer.Back.Services
{
    public class RunService
    {
        [Inject] private readonly IRunApi _api;

        public async UniTask<Result<SessionStartResponse>> StartSession(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _api.StartSession(cancellationToken);
                return Result<SessionStartResponse>.Success(response);
            }
            catch
            {
                return Result<SessionStartResponse>.Fail("Failed to start session");
            }
        }

        public async UniTask<Result<SessionFinishedResponse>> FinishSession(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _api.FinishSession(cancellationToken);
                return Result<SessionFinishedResponse>.Success(response);
            }
            catch
            {
                return Result<SessionFinishedResponse>.Fail("Failed to finish session");
            }
        }

        public async UniTask<Result<CancelSessionResponse>> CancelSession(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _api.CancelSession(cancellationToken);
                return Result<CancelSessionResponse>.Success(response);
            }
            catch
            {
                return Result<CancelSessionResponse>.Fail("Failed to cancel session");
            }
        }

        public async UniTask<Result<RunFinishResponse>> FinishRun(RunFinishRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _api.FinishRun(request, cancellationToken);
                return Result<RunFinishResponse>.Success(response);
            }
            catch
            {
                return Result<RunFinishResponse>.Fail("Failed to finish run");
            }
        }

        public async UniTask<Result<CancelRunResponse>> CancelRun(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _api.CancelRun(cancellationToken);
                return Result<CancelRunResponse>.Success(response);
            }
            catch
            {
                return Result<CancelRunResponse>.Fail("Failed to cancel run");
            }
        }

        public async UniTask<CurrentRunResponse> GetCurrentRun(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _api.GetCurrentRun(cancellationToken);
                return response;
            }
            catch
            {
                return null;
            }
        }

        public async UniTask<List<RunHistoryResponse>> GetRunHistory(RunHistoryRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _api.GetRunHistory(request, cancellationToken);
                return response;
            }
            catch
            {
                return null;
            }
        }
    }
}
