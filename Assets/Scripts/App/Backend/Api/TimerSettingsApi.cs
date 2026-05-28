using App.Backend.Config;
using App.Backend.Models;
using Cysharp.Threading.Tasks;
using PT.Tools.Http;
using System.Threading;
using Zenject;

namespace App.Backend.Api
{
    public class TimerSettingsApi : ITimerSettingsApi
    {
        [Inject] private readonly IHttpClient _http;
        [Inject] private readonly ApiConfig _apiConfig;

        public UniTask<SettingsResponse> GetTimerSettings(CancellationToken cancellationToken = default)
            => _http.Get<SettingsResponse>(_apiConfig.GetTimerSettings, cancellationToken);

        public UniTask<SettingsResponse> SetTimerSettings(SettingsRequest request, CancellationToken cancellationToken = default)
            => _http.Post<SettingsResponse>(_apiConfig.SetTimerSettings, request, cancellationToken);
    }
}
