using App.Timer.Back.Config;
using PT.GameplayAdditional.Progression;
using PT.Logic.Configs;
using PT.Tools.Addressables;
using PT.Tools.Http;
using PT.Tools.Leaderboard;
using PT.Tools.Settings.Configs;
using PT.Tools.Tutorials.Configs;
using UnityEngine;
using Zenject;

namespace PT.Logic.Dependency.ProjectContext
{
    [CreateAssetMenu(menuName = "Installers/ConfigsInstaller")]
    public class GameConfigsInstaller : ScriptableObjectInstaller<GameConfigsInstaller>
    {
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private LanguageConfig languageConfig;
        [SerializeField] private AudioConfig audioConfig;
        [SerializeField] private LeaderboardConfig leaderboardConfig;
        [SerializeField] private AssetsConfig assetsConfig;
        [SerializeField] private AdConfig adConfig;
        [SerializeField] private HttpClientConfig httpClientConfig;
        [Space]
        [SerializeField] private TutorialsSequencesConfig tutorialsSequencesConfig;
        [SerializeField] private StagesConfig stagesConfig;
        [SerializeField] private SettingsInfosConfig settingsInfosConfig;
        [SerializeField] private ApiConfig apiConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<GameConfig>().FromInstance(gameConfig).AsSingle();
            Container.Bind<LanguageConfig>().FromInstance(languageConfig).AsSingle();
            Container.Bind<AudioConfig>().FromInstance(audioConfig).AsSingle();
            Container.Bind<LeaderboardConfig>().FromInstance(leaderboardConfig).AsSingle();
            Container.Bind<AssetsConfig>().FromInstance(assetsConfig).AsSingle();
            Container.Bind<AdConfig>().FromInstance(adConfig).AsSingle();
            Container.Bind<HttpClientConfig>().FromInstance(httpClientConfig).AsSingle();

            Container.Bind<TutorialsSequencesConfig>().FromInstance(tutorialsSequencesConfig).AsSingle();
            Container.Bind<StagesConfig>().FromInstance(stagesConfig).AsSingle();
            Container.Bind<SettingsInfosConfig>().FromInstance(settingsInfosConfig).AsSingle();
            
            Container.Bind<ApiConfig>().FromInstance(apiConfig).AsSingle();
        }
    }
}