using System;
using System.Text;
using System.Threading;
using App.Timer.States;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using PT.Tools.Debugging;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace PT.Tools.Http
{
    public class UnityHttpClient : IHttpClient
    {
        [Inject] private readonly HttpClientConfig _config;
        [Inject] private readonly IAuthStorage _auth;
        [Inject] private readonly InternetState _internetState;

        public async UniTask<T> Get<T>(string url, CancellationToken cancellationToken = default)
        {
            var request = UnityWebRequest.Get(BuildUrl(url));
            Prepare(request);

            return await Send<T>(request, url, cancellationToken);
        }

        public async UniTask<T> Post<T>(string url, object body = null, CancellationToken cancellationToken = default)
        {
            var request = CreateWithBody("POST", url, body);
            return await Send<T>(request, url, cancellationToken);
        }

        public async UniTask Post(string url, object body = null, CancellationToken cancellationToken = default)
        {
            var request = CreateWithBody("POST", url, body);
            await SendRaw(request, url, cancellationToken);
        }

        public async UniTask<T> Put<T>(string url, object body = null, CancellationToken cancellationToken = default)
        {
            var request = CreateWithBody("PUT", url, body);
            return await Send<T>(request, url, cancellationToken);
        }

        public async UniTask Delete(string url, CancellationToken cancellationToken = default)
        {
            var request = new UnityWebRequest(BuildUrl(url), "DELETE");
            request.downloadHandler = new DownloadHandlerBuffer();

            Prepare(request);

            await SendRaw(request, url, cancellationToken);
        }

        private string BuildUrl(string url) => $"{_config.BaseUrl}{url}";

        private UnityWebRequest CreateWithBody(string method, string url, object body)
        {
            var json = body == null ? "" : JsonConvert.SerializeObject(body);
            var bytes = Encoding.UTF8.GetBytes(json);

            var request = new UnityWebRequest(BuildUrl(url), method);

            request.uploadHandler = new UploadHandlerRaw(bytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Prepare(request);

            return request;
        }

        private void Prepare(UnityWebRequest request)
        {
            request.timeout = _config.TimeoutSeconds;

            var token = _auth.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.SetRequestHeader("Authorization", $"Bearer {token}");
            }
        }

        private async UniTask<T> Send<T>(UnityWebRequest request, string url, CancellationToken cancellationToken)
        {
            DebugManager.Log(DebugCategory.Backend, $"HTTP {request.method} -> {url}");

            var text = await Send(request, url, cancellationToken);
            
            try
            {
                return JsonConvert.DeserializeObject<T>(text);
            }
            catch (Exception e)
            {
                DebugManager.Log(DebugCategory.Backend, $"JSON parse error: {e.Message}", LogType.Error);
                throw;
            }
        }
        private async UniTask SendRaw(UnityWebRequest request, string url, CancellationToken cancellationToken)
        {
            DebugManager.Log(DebugCategory.Backend, $"RAW HTTP {request.method} -> {url}");

            await Send(request, url, cancellationToken);
        }
        private async UniTask<string> Send(UnityWebRequest request, string url, CancellationToken cancellationToken)
        {
            try
            {
                await request.SendWebRequest().WithCancellation(cancellationToken);
            }
            catch (OperationCanceledException e)
            {
                request.Abort();
                DebugManager.Log(DebugCategory.Backend, $"Request was aborted : {request.method} {url}");
                throw;
            }
            catch (Exception e)
            {
                request.Abort();
                DebugManager.Log(DebugCategory.Backend, $"Request exception details: {e}", LogType.Error);
            }
            
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError)
            {
                if (_internetState.IsConnected.Value) DebugManager.Log(DebugCategory.Internet, $"Internet connection lost during request to {url}", LogType.Warning);
                _internetState.IsConnected.Value = false;
                throw new ApiException("Network error", request.responseCode);
            }
            if (!_internetState.IsConnected.Value) DebugManager.Log(DebugCategory.Internet, "Internet connection confirmed successful for request");
            _internetState.IsConnected.Value = true;
            
            var text = request.downloadHandler?.text;

            DebugManager.Log(DebugCategory.Backend, $"HTTP RESPONSE ({request.responseCode}) -> {text}");

            HandleErrors(request, text);

            return text;
        }

        private void HandleErrors(UnityWebRequest request, string body)
        {
            if (request.result == UnityWebRequest.Result.Success) return;

            try
            {
                var error = JsonConvert.DeserializeObject<ApiErrorResponse>(body);

                throw new ApiException(error?.error ?? "Unknown error", request.responseCode);
            }
            catch
            {
                throw new ApiException(request.error, request.responseCode);
            }
        }
    }
}