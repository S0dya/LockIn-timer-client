using UnityEngine;

namespace PT.Tools.Http.Storage
{
    public class PlayerPrefsAuthStorage : IAuthStorage
    {
        private const string KEY = "auth_token";

        public void SetToken(string token)
        {
            PlayerPrefs.SetString(KEY, token);
            PlayerPrefs.Save();
        }

        public string GetToken()
        {
            return PlayerPrefs.GetString(KEY, string.Empty);
        }

        public void Clear()
        {
            PlayerPrefs.DeleteKey(KEY);
        }
    }
}