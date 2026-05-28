using App.Backend.Api;
using App.Backend.Config;
using App.Backend.Services;
using UnityEngine;
using Zenject;

namespace App.Backend
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
