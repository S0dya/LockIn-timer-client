using App.Timer.Back.Api;
using App.Timer.Back.Models;
using Cysharp.Threading.Tasks;
using System.Threading;
using Zenject;

namespace App.Timer.Back.Services
{
    public class TimerSettingsService
    {
        [Inject] private readonly ITimerSettingsApi _api;

        public UniTask<SettingsResponse> GetTimerSettings(CancellationToken cancellationToken = default)
            => _api.GetTimerSettings(cancellationToken);

        public UniTask<SettingsResponse> SetTimerSettings(SettingsRequest request, CancellationToken cancellationToken = default)
            => _api.SetTimerSettings(request, cancellationToken);
    }
}
