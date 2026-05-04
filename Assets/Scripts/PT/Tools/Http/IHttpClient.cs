using Cysharp.Threading.Tasks;
using System.Threading;

namespace PT.Tools.Http
{
    public interface IHttpClient
    {
        UniTask<T> Get<T>(string url, CancellationToken cancellationToken = default);
        UniTask<T> Post<T>(string url, object body = null, CancellationToken cancellationToken = default);
        UniTask Post(string url, object body = null, CancellationToken cancellationToken = default);
        UniTask<T> Put<T>(string url, object body = null, CancellationToken cancellationToken = default);
        UniTask Delete(string url, CancellationToken cancellationToken = default);
    }
}