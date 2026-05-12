using System;
using UnityEngine;
using UnityEngine.UI;

namespace App.Timer.Run
{
    public class RunActionsView : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private Button[] timerSettingsButtons;
        [Header("Run")]
        [SerializeField] private GameObject runActionsObj;
        [SerializeField] private Button startSessionButton;
        [SerializeField] private Button cancelRunButton;
        [Header("Session")]
        [SerializeField] private GameObject sessionActionsObj;
        [SerializeField] private Button cancelSessionButton;
        
        public event Action OnStartSession;
        public event Action OnCancelRun;
        public event Action OnTimerSettings;
        public event Action OnCancelSession;

        private void Start()
        {
            startSessionButton.onClick.AddListener(OnStartSessionPressed);
            cancelRunButton.onClick.AddListener(OnCancelRunPressed);
            foreach (var timerSettingsButton in timerSettingsButtons) timerSettingsButton.onClick.AddListener(OnTimerSettingsPressed);
            cancelSessionButton.onClick.AddListener(OnCancelSessionPressed);
        }

        public void ShowSession()
        {
            sessionActionsObj.SetActive(true);
            runActionsObj.SetActive(false);
        }
        public void ShowRun()
        {
            sessionActionsObj.SetActive(false);
            runActionsObj.SetActive(true);
        } 
        
        private void OnStartSessionPressed() // add more visuals later
        {
            OnStartSession?.Invoke();
        }
        private void OnCancelRunPressed()
        {
            OnCancelRun?.Invoke();
        }
        private void OnTimerSettingsPressed()
        {
            OnTimerSettings?.Invoke();
        }
        private void OnCancelSessionPressed()
        {
            OnCancelSession?.Invoke();
        }
    }
}