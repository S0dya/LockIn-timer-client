using PT.GameplayAdditional.Progression;
using PT.Logic.Dependency.Signals;
using PT.Tools.Http;
using PT.Tools.Http.Storage;
using PT.Tools.Notifications;
using PT.Tools.Notifications.Platforms;
using Zenject;

namespace PT.Logic.Dependency.ProjectContext
{
    public class ProjectInstaller : BaseProjectInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.Bind<StageProvider>().AsSingle();
            Container.Bind<InternetState>().AsSingle();
            Container.Bind<IHttpClient>().To<UnityHttpClient>().AsSingle();
            Container.Bind<IAuthStorage>().To<PlayerPrefsAuthStorage>().AsSingle();
            
            Container.Bind<NotificationsManager>().AsSingle();

#if UNITY_ANDROID
            Container.Bind<INotificationService>().To<AndroidNotificationService>().AsSingle();
#elif UNITY_STANDALONE_WIN
            Container.Bind<INotificationService>().To<WindowsNotificationService>().AsSingle();
#endif
            
            Container.DeclareSignal<SessionStartedSignal>();
            Container.DeclareSignal<SessionFinishedSignal>();
        }
    }
}
