#if UNITY_IOS || UNITY_ANDROID
using Cysharp.Threading.Tasks;
using PT.Backend.Interfaces;
#endif

namespace PT.Backend.GP
{
#if UNITY_IOS || UNITY_ANDROID

    public class GooglePlayAuthentificationService : IAuthentificationService
    {
        public bool IsSignedIn { get; private set; }
        public string PlayerId { get; private set; }
        public string DisplayName { get; private set; }

        public UniTask SignIn()
        {
            /*
            Google Play Authentication SignIn implementation is intentionally disabled.
            */
            IsSignedIn = false;
            PlayerId = null;
            DisplayName = null;
            return UniTask.CompletedTask;
        }

        public UniTask SetDisplayName(string name)
        {
            /*
            Google Play Authentication SetDisplayName implementation is intentionally disabled.
            */
            return UniTask.CompletedTask;
        }
    }
#endif
}
