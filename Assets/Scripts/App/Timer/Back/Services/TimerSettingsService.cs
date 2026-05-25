using System;
using App.Timer.Back.Api;
using App.Timer.Back.Models;
using Cysharp.Threading.Tasks;
using System.Threading;
using PT.Tools.Http;
using Zenject;

namespace App.Timer.Back.Services
{
    public class TimerSettingsService : BaseService
    {
        [Inject] private readonly ITimerSettingsApi _api;
        [Inject] private readonly AppConfig _appConfig;

        private SettingsResponse _cachedSettingsResponse;
        private DateTime _cachedSettingsTimestamp;

        public async UniTask<Result<SettingsResponse>> GetTimerSettings(CancellationToken cancellationToken = default)
        {
            if (_cachedSettingsResponse != null && 
                DateTime.UtcNow - _cachedSettingsTimestamp < TimeSpan.FromSeconds(_appConfig.CacheTimerSettingsDurationSeconds))
            {
                return Result<SettingsResponse>.Success(_cachedSettingsResponse);
            }
            
            var result = await Execute(_api.GetTimerSettings(cancellationToken), "Failed to get timer settings");
            
            if (result.IsSuccess)
                UpdateCachedSettings(result.Value);
            
            return result;
        }

        public async UniTask<Result<SettingsResponse>> SetTimerSettings(SettingsRequest request, CancellationToken cancellationToken = default)
        {
            var result = await Execute(_api.SetTimerSettings(request, cancellationToken), "Failed to set timer settings");

            if (result.IsSuccess)
                UpdateCachedSettings(result.Value);
            
            return result;
        }
        
        private void UpdateCachedSettings(SettingsResponse settings)
        {
            if (settings == null) return;
            
            _cachedSettingsResponse = settings;
            _cachedSettingsTimestamp = DateTime.UtcNow;
        }
    }
}
