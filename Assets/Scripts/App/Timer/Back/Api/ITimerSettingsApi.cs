using App.Timer.Back.Models;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace App.Timer.Back.Api
{
    public interface ITimerSettingsApi
    {
        UniTask<SettingsResponse> GetTimerSettings(CancellationToken cancellationToken = default);
        UniTask<SettingsResponse> SetTimerSettings(SettingsRequest request, CancellationToken cancellationToken = default);
    }
}
