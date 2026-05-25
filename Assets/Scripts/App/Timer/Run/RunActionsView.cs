using System;
using App.Timer.States;
using DG.Tweening;
using PT.Tools.Helper;
using PT.UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace App.Timer.Run
{
    public class RunActionsView : MonoBehaviour
    {
        [SerializeField] private Button[] timerSettingsButtons;
        [SerializeField] private SerializableKeyValue<RunStatus, GameObject[]> runStatusToView;
        [Space]
        [SerializeField] private HoldButton cancelRunButton;
        [SerializeField] private Button submitRunButton;
        [SerializeField] private CanvasGroup submitRunCg;
        [Space]
        [SerializeField] private Button[] startSessionButtons;
        [Space]
        [SerializeField] private Transform playButtonTransform;
        [SerializeField] private float playButtonPunchScale = 1.12f;
        [SerializeField] private float playButtonAnimationDuration = 0.2f;
        [Space]
        [SerializeField] private HoldButton cancelSessionButton;
        [Space]
        [SerializeField] private Button runHistoryButton;
        
        public event Action OnStartSession;
        public event Action OnCancelRun;
        public event Action OnTimerSettings;
        public event Action OnCancelSession;
        public event Action OnSubmitRun;
        public event Action OnOpenHistory;

        private Tween _playButtonTween;

        private bool _wasPlaying;

        private void Start()
        {
            foreach (var startSessionButton in startSessionButtons) startSessionButton.onClick.AddListener(OnStartSessionPressed);
            cancelRunButton.OnCompleted += OnCancelRunPressed;
            foreach (var timerSettingsButton in timerSettingsButtons) timerSettingsButton.onClick.AddListener(OnTimerSettingsPressed);
            cancelSessionButton.OnCompleted += OnCancelSessionPressed;
            submitRunButton.onClick.AddListener(OnSubmitRunPressed);
            runHistoryButton.onClick.AddListener(OnOpenHistoryPressed);
            
            foreach (var kvp in runStatusToView.Dictionary) kvp.Value.SetActive(false);
        }

        public void UpdateView(RunState run)
        {
            foreach (var kvp in runStatusToView.Dictionary) kvp.Value.SetActive(false);
            runStatusToView.Dictionary[run.RunStatus].SetActive(true);

            switch (run.RunStatus)
            {
                case RunStatus.Active:
                    if (!_wasPlaying)
                    {
                        PlayButtonPressedAnimation();
                    }
                    _wasPlaying = true;
                    break;
                case RunStatus.Idle:
                case RunStatus.None:
                    _wasPlaying = false;
                    _playButtonTween?.Kill();
                    playButtonTransform.localScale = Vector3.one;
                    break;
            }
        }

        public void SetSubmitButtonInteractable(bool isInteractable)
        {
            submitRunCg.interactable = isInteractable;
        }
        
        private void PlayButtonPressedAnimation()
        {
            _playButtonTween?.Kill();

            playButtonTransform.localScale = Vector3.one;

            _playButtonTween = DOTween.Sequence()
                .Append(playButtonTransform.DOScale(playButtonPunchScale, playButtonAnimationDuration * 0.5f))
                .Append(playButtonTransform.DOScale(0f, playButtonAnimationDuration * 0.5f))
                .SetEase(Ease.OutQuad);
        }

        private void OnStartSessionPressed()
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

        private void OnSubmitRunPressed()
        {
            OnSubmitRun?.Invoke();
        }
        
        private void OnOpenHistoryPressed()
        {
            OnOpenHistory?.Invoke();
        }

        private void OnDestroy()
        {
            _playButtonTween?.Kill();
        }
    }
}