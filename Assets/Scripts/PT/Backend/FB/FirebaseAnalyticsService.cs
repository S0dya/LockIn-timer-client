#if UNITY_IOS || UNITY_ANDROID
using System.Collections.Generic;
using PT.Backend.Interfaces;

namespace PT.Backend.FB
{
    public class FirebaseAnalyticsService : IAnalyticsService
    {
        public void Log(string eventName)
        {
            /*
            Firebase Analytics Log(eventName) implementation is intentionally disabled.
            */
        }

        public void Log(string eventName, Dictionary<string, object> parameters)
        {
            /*
            Firebase Analytics Log(eventName, parameters) implementation is intentionally disabled.
            */
        }
    }
}
#endif