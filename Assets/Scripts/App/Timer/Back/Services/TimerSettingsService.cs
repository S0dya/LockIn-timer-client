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

        public async UniTask<Result<SettingsResponse>> GetTimerSettings(CancellationToken cancellationToken = default)
        {
            return await Execute(_api.GetTimerSettings(cancellationToken), "Failed to get timer settings");
        }

        public async UniTask<Result<SettingsResponse>> SetTimerSettings(SettingsRequest request, CancellationToken cancellationToken = default)
        {
            return await Execute(_api.SetTimerSettings(request, cancellationToken), "Failed to set timer settings");
        }
    }
}
