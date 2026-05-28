using System;
using App.Backend.Api;
using App.Backend.Models;
using Cysharp.Threading.Tasks;
using System.Threading;
using PT.Tools.Http;
using PT.Tools.Http.Storage;
using Zenject;

namespace App.Backend.Services
{
    public class AuthService : BaseService
    {
        [Inject] private readonly IAuthApi _api;
        [Inject] private readonly IAuthStorage _authStorage;

        public async UniTask<Result<AuthResponse>> Login(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var result = await Execute(_api.Login(request, cancellationToken), "Failed to login");
            
            if (result.IsSuccess)
                _authStorage.SetToken(result.Value.Token);

            return result;
        }

        public async UniTask<Result<AuthResponse>> Register(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var result = await Execute(_api.Register(request, cancellationToken), "Failed to register");
            
            if (result.IsSuccess)
                _authStorage.SetToken(result.Value.Token);

            return result;
        }
        public async UniTask<Result<UserResponse>> GetCurrentUser(CancellationToken cancellationToken = default)
        {
            var token = _authStorage.GetToken();

            if (String.IsNullOrEmpty(token)) return Result<UserResponse>.Fail("Token is null");

            var result = await Execute(_api.GetCurrentUser(cancellationToken), "Failed to get current user");
            
            if (!result.IsSuccess) 
                _authStorage.Clear();

            return result;
        }
    }
}
