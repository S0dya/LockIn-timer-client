using PT.Logic.PersistentScene;
using UnityEngine;

namespace PT.Logic.Dependency.Signals
{
    public class ShowAdSignal { }
    public class AdOpenedSignal { }
    public class AdClosedSignal { }
    public class AdCountdownStartSignal { }
    public class AdCountdownStopSignal { }

    public class SceneLoadSignal
    {
        public SceneNameEnum SceneName { get; private set; }
        public SceneLoadSignal(SceneNameEnum sceneName) { SceneName = sceneName; }
    }
    public class SceneUnloadedSignal
    {
        public SceneNameEnum SceneName { get; private set; }
        public SceneUnloadedSignal(SceneNameEnum sceneName) { SceneName = sceneName; }
    }
    
    public class GameMenuOpenedSignal { }
    public class GameMenuClosedSignal { }

    public class SessionStartedSignal
    {
        public int Duration { get; private set; }
        public SessionStartedSignal(int duration) { Duration = duration; }
    }
    public class SessionFinishedSignal { }
}