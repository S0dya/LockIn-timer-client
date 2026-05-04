using App.Timer.Back.Api;
using App.Timer.Back.Config;
using App.Timer.Back.Services;
using UnityEngine;
using Zenject;

namespace App.Timer.Back
{
    public class ServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IAuthApi>().To<AuthApi>().AsSingle();
            Container.Bind<ITimerSettingsApi>().To<TimerSettingsApi>().AsSingle();
            Container.Bind<IRunApi>().To<RunApi>().AsSingle();
            
            Container.Bind<AuthService>().AsSingle();
            Container.Bind<TimerSettingsService>().AsSingle();
            Container.Bind<RunService>().AsSingle();
        }
    }
}
