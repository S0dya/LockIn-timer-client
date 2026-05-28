using UnityEngine;

namespace App.Backend.Config
{
    [CreateAssetMenu(menuName = "Configs/ApiConfig", fileName = "ApiConfig")]
    public class ApiConfig : ScriptableObject
    {
        [Header("Auth Endpoints")]
        [SerializeField] private string login = "/auth/login";
        [SerializeField] private string register = "/auth/register";
        [SerializeField] private string getCurrentUser = "/auth/get-current-user";
     
        [Header("Internet Check")]
        [SerializeField] private string checkInternet = "/health";

        [Header("Timer Settings Endpoints")]
        [SerializeField] private string getTimerSettings = "/timer/settings";
        [SerializeField] private string setTimerSettings = "/timer/settings";
        
        [Header("Timer Run Endpoints")]
        [SerializeField] private string startSession = "/timer/run/start-session";
        [SerializeField] private string finishSession = "/timer/run/finish-session";
        [SerializeField] private string cancelSession = "/timer/run/cancel-session";
        [SerializeField] private string finishRun = "/timer/run/finish-run";
        [SerializeField] private string cancelRun = "/timer/run/cancel-run";
        [SerializeField] private string currentRun = "/timer/run/get-current-run";
        [SerializeField] private string runHistory = "/timer/run/get-run-history";
        
        public string Login => login;
        public string Register => register;
        public string GetCurrentUser => getCurrentUser;
        
        public string CheckInternet => checkInternet;
        
        public string GetTimerSettings => getTimerSettings;
        public string SetTimerSettings => setTimerSettings;
        
        public string StartSession => startSession;
        public string FinishSession => finishSession;
        public string CancelSession => cancelSession;
        public string FinishRun => finishRun;
        public string CancelRun => cancelRun;
        public string CurrentRun => currentRun;
        public string RunHistory => runHistory;
    }
}
