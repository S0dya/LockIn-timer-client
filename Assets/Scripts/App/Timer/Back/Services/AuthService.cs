using App.Timer.Back.Api;
using App.Timer.Back.Models;
using Cysharp.Threading.Tasks;
using System.Threading;
using Zenject;

namespace App.Timer.Back.Services
{
    public class AuthService
    {
        [Inject] private readonly IAuthApi _api;

        public UniTask<AuthResponse> Login(LoginRequest request, CancellationToken cancellationToken = default)
            => _api.Login(request, cancellationToken);

        public UniTask<AuthResponse> Register(RegisterRequest request, CancellationToken cancellationToken = default)
            => _api.Register(request, cancellationToken);
    }
}
