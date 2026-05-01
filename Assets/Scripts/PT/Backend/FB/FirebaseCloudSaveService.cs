#if UNITY_IOS || UNITY_ANDROID

using Cysharp.Threading.Tasks;
using PT.Backend.Interfaces;
using PT.Logic.Save;

namespace PT.Backend.FB
{
    public class FirebaseCloudSaveService : ICloudSaveService
    {
        public async UniTask<SaveData> Load()
        {
            /*
            Firebase Cloud Save Load implementation is intentionally disabled.
            */
            await UniTask.CompletedTask;
            return null;
        }

        public async UniTask Save(SaveData data)
        {
            /*
            Firebase Cloud Save Save implementation is intentionally disabled.
            */
            await UniTask.CompletedTask;
        }
    }
}
#endif