using App.Backend.Config;
using App.Backend.Models;
using Cysharp.Threading.Tasks;
using PT.Tools.Http;
using System.Threading;
using Zenject;

namespace App.Backend.Api
{
    public class AuthApi : IAuthApi
    {
        [Inject] private readonly IHttpClient _http;
        [Inject] private readonly ApiConfig _apiConfig;

        public UniTask<AuthResponse> Login(LoginRequest request, CancellationToken cancellationToken = default)
            => _http.Post<AuthResponse>(_apiConfig.Login, request, cancellationToken);

        public UniTask<AuthResponse> Register(RegisterRequest request, CancellationToken cancellationToken = default)
            => _http.Post<AuthResponse>(_apiConfig.Register, request, cancellationToken);

        public UniTask<UserResponse> GetCurrentUser(CancellationToken cancellationToken = default)
            => _http.Get<UserResponse>(_apiConfig.GetCurrentUser, cancellationToken);
    }
}
