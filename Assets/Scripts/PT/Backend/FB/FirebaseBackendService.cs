#if UNITY_IOS || UNITY_ANDROID

using Cysharp.Threading.Tasks;
using PT.Backend.Interfaces;

namespace PT.Backend.FB
{
    public class FirebaseBackendService : IBackendService
    {
        public bool IsReady { get; private set; }

        public async UniTask Init()
        {
            /*
            Firebase implementation is intentionally disabled.
            Original initialization and dependency checks were removed from runtime.
            */
            IsReady = false;
            await UniTask.CompletedTask;
        }
    }
}
#endif