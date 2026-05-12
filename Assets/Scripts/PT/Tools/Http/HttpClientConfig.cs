using UnityEngine;

namespace PT.Tools.Http
{
    [CreateAssetMenu(menuName = "Configs/HttpClientConfig", fileName = "HttpClientConfig")]
    public class HttpClientConfig : ScriptableObject
    {
        [SerializeField] private string baseUrl;
        [SerializeField] private int timeoutSeconds = 10;
        [SerializeField] private int retryIntervalSeconds = 5;
        [SerializeField] private int manualRetryCooldownSeconds = 3;
        
        public string BaseUrl => baseUrl;
        public int TimeoutSeconds => timeoutSeconds;
        public int RetryIntervalSeconds => retryIntervalSeconds;
        public int ManualRetryCooldownSeconds => manualRetryCooldownSeconds;
    }
}