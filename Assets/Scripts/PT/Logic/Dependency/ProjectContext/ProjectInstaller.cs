using App.Timer.States;
using PT.GameplayAdditional.Progression;
using PT.Logic.Dependency.Signals;
using PT.Tools.Http;
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
        }
    }
}
