#if UNITY_IOS || UNITY_ANDROID
using PT.Backend.Interfaces;

namespace PT.Backend.FB
{
    public class FirebaseRemoteConfigService : IRemoteConfigService
    {
        public int GetInt(string key, int defaultValue)
        {
            /*
            Firebase Remote Config GetInt implementation is intentionally disabled.
            */
            return defaultValue;
        }

        public float GetFloat(string key, float defaultValue)
        {
            /*
            Firebase Remote Config GetFloat implementation is intentionally disabled.
            */
            return defaultValue;
        }

        public bool GetBool(string key, bool defaultValue)
        {
            /*
            Firebase Remote Config GetBool implementation is intentionally disabled.
            */
            return defaultValue;
        }

        public string GetString(string key, string defaultValue)
        {
            /*
            Firebase Remote Config GetString implementation is intentionally disabled.
            */
            return defaultValue;
        }
    }
}
#endif