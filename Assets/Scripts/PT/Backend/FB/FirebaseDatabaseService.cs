#if UNITY_ANDROID || UNITY_IOS
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PT.Backend.Interfaces;
using PT.Backend.Types;
using Zenject;

namespace PT.Backend.FB
{
    public class FirebaseDatabaseService : IDatabaseService, IInitializable
    {
        public void Initialize()
        {
            /*
            Firebase Database initialization is intentionally disabled.
            */
        }

        public async UniTask<T> GetData<T>(string path)
        {
            /*
            Firebase Database GetData implementation is intentionally disabled.
            */
            await UniTask.CompletedTask;
            return default;
        }

        public async UniTask<bool> SetDataAsync<T>(string path, T data)
        {
            /*
            Firebase Database SetDataAsync implementation is intentionally disabled.
            */
            await UniTask.CompletedTask;
            return false;
        }

        public async UniTask<IReadOnlyList<T>> Query<T>(DatabaseQuery query)
        {
            /*
            Firebase Database Query implementation is intentionally disabled.
            */
            await UniTask.CompletedTask;
            return new List<T>();
        }
    }
}
#endif