using UnityEngine;

namespace PT.Tools.Http
{
    [CreateAssetMenu(menuName = "Configs/HttpClientConfig", fileName = "HttpClientConfig")]
    public class HttpClientConfig : ScriptableObject
    {
        [SerializeField] private string baseUrl;
        [SerializeField] private int timeoutSeconds = 10;
        
        public string BaseUrl => baseUrl;
        public int TimeoutSeconds => timeoutSeconds;
    }
}