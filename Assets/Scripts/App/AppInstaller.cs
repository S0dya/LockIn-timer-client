using App.Timer;
using App.Timer.Back.Api;
using App.Timer.States;
using App.Timer.Windows;
using App.Timer.Back.Services;
using App.Timer.Login;
using App.Timer.Settings;
using App.Timer.Run;
using App.Timer.Back.Config;
using PT.Tools.Http;
using UnityEngine;
using Zenject;

namespace App
{
    public class AppInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AppBootstrapper>().AsSingle();
            Container.BindInterfacesAndSelfTo<AppSynchronizationService>().AsSingle();
            
            Container.Bind<AppState>().AsSingle();
            
            Container.Bind<AuthService>().AsSingle();
            Container.Bind<RunService>().AsSingle();
            Container.Bind<TimerSettingsService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<AuthViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<RunViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<TimerSettingsViewModel>().AsSingle();
            
            Container.Bind<IAuthApi>().To<AuthApi>().AsSingle();
            Container.Bind<IRunApi>().To<RunApi>().AsSingle();
            Container.Bind<ITimerSettingsApi>().To<TimerSettingsApi>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<RequestLoadingManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<RequestErrorManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<InternetConnectionManager>().AsSingle();
            
            Container.Bind<TimerSettingsViewWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<RunActionsView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<RunSubmitViewWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<RunTimerClockView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<SessionFinishedViewWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<AuthViewWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<RequestErrorWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<InternetConnectionWindow>().FromComponentInHierarchy().AsSingle();
        }
    }
}