using App.Backend.Models;
using Cysharp.Threading.Tasks;
using PT.Tools.Http;
using System.Threading;

namespace App.Backend.Api
{
    public interface IAuthApi
    {
        UniTask<AuthResponse> Login(LoginRequest request, CancellationToken cancellationToken = default);
        UniTask<AuthResponse> Register(RegisterRequest request, CancellationToken cancellationToken = default);
        UniTask<UserResponse> GetCurrentUser(CancellationToken cancellationToken = default);
    }
}
