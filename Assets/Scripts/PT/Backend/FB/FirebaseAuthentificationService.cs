#if UNITY_ANDROID || UNITY_IOS
using Cysharp.Threading.Tasks;
using PT.Backend.Interfaces;
#endif

namespace PT.Backend.FB
{
#if UNITY_ANDROID || UNITY_IOS
    public class FirebaseAuthentificationService : IAuthentificationService
    {
        public bool IsSignedIn => false;
        public string PlayerId => null;
        public string DisplayName => null;

        public async UniTask SignIn()
        {
            /*
            Firebase Authentication SignIn implementation is intentionally disabled.
            */
            await UniTask.CompletedTask;
        }

        public async UniTask SetDisplayName(string newName)
        {
            /*
            Firebase Authentication SetDisplayName implementation is intentionally disabled.
            */
            await UniTask.CompletedTask;
        }
    }
#endif
}
