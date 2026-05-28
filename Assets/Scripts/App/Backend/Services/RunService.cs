using System;
using App.Backend.Api;
using App.Backend.Models;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using App.Core;
using PT.Tools.Debugging;
using PT.Tools.Http;
using UnityEngine;
using Zenject;

namespace App.Backend.Services
{
    public class RunService : BaseService
    {
        [Inject] private readonly IRunApi _api;
        [Inject] private readonly AppConfig _appConfig;

        private readonly Dictionary<int, RunHistoryResponse> _cachedRunHistory = new();
        private DateTime _cachedRunHistoryTimestamp;
        
        public async UniTask<Result<SessionStartResponse>> StartSession(CancellationToken cancellationToken = default)
        {
            return await Execute(_api.StartSession(cancellationToken), "Failed to start session");
        }

        public async UniTask<Result<SessionFinishedResponse>> FinishSession(CancellationToken cancellationToken = default)
        {
            return await Execute(_api.FinishSession(cancellationToken), "Failed to finish session");
        }

        public async UniTask<Result<CancelSessionResponse>> CancelSession(CancellationToken cancellationToken = default)
        {
            return await Execute(_api.CancelSession(cancellationToken), "Failed to cancel session");
        }

        public async UniTask<Result<RunFinishResponse>> FinishRun(RunFinishRequest request, CancellationToken cancellationToken = default)
        {
            var result = await Execute(_api.FinishRun(request, cancellationToken), "Failed to finish run");

            if (result.IsSuccess)
                _cachedRunHistory.Clear();

            return result;
        }

        public async UniTask<Result<CancelRunResponse>> CancelRun(CancellationToken cancellationToken = default)
        {
            return await Execute(_api.CancelRun(cancellationToken), "Failed to cancel run");
        }

        public async UniTask<Result<CurrentRunResponse>> GetCurrentRun(CancellationToken cancellationToken = default)
        {
            return await Execute(_api.GetCurrentRun(cancellationToken), "Failed to get current run");
        }

        public async UniTask<Result<List<RunHistoryResponse>>> GetRunHistory(RunHistoryRequest request, CancellationToken cancellationToken = default)
        {
            var cacheExpired = DateTime.UtcNow - _cachedRunHistoryTimestamp > TimeSpan.FromSeconds(_appConfig.CacheRunHistoryDurationSeconds);
            
            if (cacheExpired)
            {
                DebugManager.Log(DebugCategory.TimerRun, $"Cache expired, clearing cache...");
                
                _cachedRunHistory.Clear();
            }
            else
            {
                var isFullyCached = true;

                for (int i = request.Offset; i < request.Offset + request.Limit; i++)
                {
                    if (_cachedRunHistory.ContainsKey(i)) continue;

                    DebugManager.Log(DebugCategory.TimerRun, $"Cached run history not found for index {i}");
                    
                    isFullyCached = false;
                    break;
                }
                
                if (isFullyCached)
                {
                    DebugManager.Log(DebugCategory.TimerRun, $"Run history fully cached, returning cached data");
                    
                    var cachedRunHistory = new List<RunHistoryResponse>();
                    for (int i = request.Offset; i < request.Offset + request.Limit; i++) 
                        cachedRunHistory.Add(_cachedRunHistory[i]);
                    
                    return Result<List<RunHistoryResponse>>.Success(cachedRunHistory);
                }   
            }
            
            var result = await Execute(_api.GetRunHistory(request, cancellationToken), "Failed to get run history");

            if (!result.IsSuccess) return result;

            for (int i = 0; i < result.Value.Count; i++)
                _cachedRunHistory[request.Offset + i] = result.Value[i];
            
            _cachedRunHistoryTimestamp = DateTime.UtcNow;

            return result;
        }
    }
}
