using System;
using UnityEngine;
using UnityEngine.UI;

namespace App.Timer.Run
{
    public class RunTimerActionButtons : MonoBehaviour
    {
        [SerializeField] private GameObject runButtonsObj;
        [SerializeField] private Button startSessionButton;
        [SerializeField] private Button cancelRunButton;
        [Space]
        [SerializeField] private GameObject sessionButtonsObj;
        [SerializeField] private Button cancelSessionButton;
        
        public event Action OnStartSession;
        public event Action OnCancelRun;
        public event Action OnCancelSession;

        private void Start()
        {
            startSessionButton.onClick.AddListener(OnStartSessionPressed);
            cancelRunButton.onClick.AddListener(OnCancelRunPressed);
            cancelSessionButton.onClick.AddListener(OnCancelSessionPressed);
            
        }

        public void ShowSession()
        {
            sessionButtonsObj.SetActive(true);
            runButtonsObj.SetActive(false);
        }
        public void ShowRun()
        {
            sessionButtonsObj.SetActive(false);
            runButtonsObj.SetActive(true);
        } 
        
        private void OnStartSessionPressed() // add more visuals later
        {
            OnStartSession?.Invoke();
        }
        private void OnCancelRunPressed()
        {
            OnCancelRun?.Invoke();
        }
        private void OnCancelSessionPressed()
        {
            OnCancelSession?.Invoke();
        }
    }
}