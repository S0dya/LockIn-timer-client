using System;
using System.Collections.Generic;
using App.Timer.States;
using DG.Tweening;
using PT.Tools.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Timer.Run
{
    public class RunActionsView : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private Button[] timerSettingsButtons;
        [SerializeField] private SerializableKeyValue<RunStatus, GameObject[]> runStatusToView;
        [Space]
        [SerializeField] private Button cancelRunButton;
        [SerializeField] private Button submitRunButton;
        [Space]
        [SerializeField] private Button[] startSessionButtons;
        [Space]
        [SerializeField] private Transform playButtonTransform;
        [SerializeField] private float playButtonPunchScale = 1.12f;
        [SerializeField] private float playButtonAnimationDuration = 0.2f;
        [Space]
        [SerializeField] private Button cancelSessionButton;
        
        public event Action OnStartSession;
        public event Action OnCancelRun;
        public event Action OnTimerSettings;
        public event Action OnCancelSession;
        public event Action OnSubmitRun;

        private Dictionary<RunStatus, GameObject[]> _runStatusToView;

        private Tween _playButtonTween;

        private bool _wasPlaying;

        private void Start()
        {
            _runStatusToView = runStatusToView.Dictionary;
            
            foreach (var startSessionButton in startSessionButtons) startSessionButton.onClick.AddListener(OnStartSessionPressed);
            cancelRunButton.onClick.AddListener(OnCancelRunPressed);
            foreach (var timerSettingsButton in timerSettingsButtons) timerSettingsButton.onClick.AddListener(OnTimerSettingsPressed);
            cancelSessionButton.onClick.AddListener(OnCancelSessionPressed);
            submitRunButton.onClick.AddListener(OnSubmitRunPressed);
        }

        public void UpdateView(RunState run)
        {
            foreach (var kvp in _runStatusToView) kvp.Value.SetActive(false);
            _runStatusToView[run.RunStatus].SetActive(true);

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

        private void OnDestroy()
        {
            _playButtonTween?.Kill();
        }
    }
}