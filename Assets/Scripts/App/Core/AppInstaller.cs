using App.Backend.Api;
using App.Core.States;
using App.Shared.Windows;
using App.Backend.Services;
using App.Features.Login;
using App.Features.Settings;
using App.Features.Run;
using App.Features.Run.History;
using App.Features.Run.Views;
using App.Features.Run.Windows;
using App.Shared.Synchronization;
using PT.Tools.Windows;
using Zenject;

namespace App.Core
{
    public class AppInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<WindowsManager>().WithId("App").FromComponentInHierarchy().AsSingle();
         
            Container.BindInterfacesAndSelfTo<AppBootstrapper>().AsSingle();
            Container.BindInterfacesAndSelfTo<AppSynchronizationService>().AsSingle();
            
            Container.Bind<AppState>().AsSingle();
            
            Container.Bind<AuthService>().AsSingle();
            Container.Bind<RunService>().AsSingle();
            Container.Bind<TimerSettingsService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<AuthViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<RunViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<RunHistoryViewModel>().AsSingle();
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
            Container.Bind<RunHistoryWindow>().FromComponentInHierarchy().AsSingle();
        }
    }
}