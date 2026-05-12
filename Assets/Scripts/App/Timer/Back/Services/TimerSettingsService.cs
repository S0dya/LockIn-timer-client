using App.Timer.Back.Api;
using App.Timer.Back.Models;
using Cysharp.Threading.Tasks;
using System.Threading;
using PT.Tools.Http;
using Zenject;

namespace App.Timer.Back.Services
{
    public class TimerSettingsService
    {
        [Inject] private readonly ITimerSettingsApi _api;

        public async UniTask<Result<SettingsResponse>> GetTimerSettings(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _api.GetTimerSettings(cancellationToken);
                return Result<SettingsResponse>.Success(response);
            }
            catch
            {
                return Result<SettingsResponse>.Fail("Failed to get timer settings");
            }
        }

        public async UniTask<Result<SettingsResponse>> SetTimerSettings(SettingsRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _api.SetTimerSettings(request, cancellationToken);
                
                // add Retrying
                
                return Result<SettingsResponse>.Success(response);
            }
            catch
            {
                return Result<SettingsResponse>.Fail("Failed to set timer settings");
            }
        }
    }
}
