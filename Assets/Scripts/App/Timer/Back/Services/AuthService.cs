using System;
using App.Timer.Back.Api;
using App.Timer.Back.Models;
using Cysharp.Threading.Tasks;
using System.Threading;
using PT.Tools.Http;
using Zenject;

namespace App.Timer.Back.Services
{
    public class AuthService
    {
        [Inject] private readonly IAuthApi _api;
        [Inject] private readonly IAuthStorage _authStorage;

        public async UniTask<Result<AuthResponse>> Login(LoginRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _api.Login(request, cancellationToken);
                
                _authStorage.SetToken(response.Token);

                return Result<AuthResponse>.Success(response);
            }
            catch
            {
                return Result<AuthResponse>.Fail("Failed to login");
            }
        }

        public async UniTask<Result<AuthResponse>> Register(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _api.Register(request, cancellationToken);
                
                _authStorage.SetToken(response.Token);

                return Result<AuthResponse>.Success(response);
            }
            catch
            {
                return Result<AuthResponse>.Fail("Failed to register");
            }
        }
        public async UniTask<Result<UserResponse>> GetCurrentUser(CancellationToken cancellationToken = default)
        {
            var token = _authStorage.GetToken();

            if (String.IsNullOrEmpty(token)) return Result<UserResponse>.Fail("Token is null");

            try
            {
                var response = await _api.GetCurrentUser(cancellationToken);
                return Result<UserResponse>.Success(response);
            }
            catch
            {
                _authStorage.Clear();
                return Result<UserResponse>.Fail("Token is null");
            }
        }
    }
}
