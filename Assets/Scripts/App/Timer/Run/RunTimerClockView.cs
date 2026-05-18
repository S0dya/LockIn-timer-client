using System.Collections.Generic;
using App.Timer.States;
using DG.Tweening;
using NaughtyAttributes;
using PT.Tools.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Timer.Run
{
    public class RunTimerClockView : MonoBehaviour
    {
        [Space]
        [SerializeField] private Image clockFillImage;
        [SerializeField] private Transform knobPivot;
        [Space]
        [SerializeField][MinMaxSlider(0.0001f, 1.5f)] private Vector2 minMaxAnimationDuration;
        [SerializeField] private Ease fillAnimationEase = Ease.Linear;
        [Space]
        [SerializeField] private TextMeshProUGUI timerText;
        [Space]
        [SerializeField] private SerializableKeyValue<RunStatus, GameObject[]> runStatusToView;

        private float _currentFill;

        private Tween _fillTween;
        private Tween _rotationTween;

        public void UpdateView(RunStatus status)
        {
            foreach (var kvp in runStatusToView.Dictionary) kvp.Value.SetActive(false);

            runStatusToView.Dictionary[status].SetActive(true);
        }

        public void SetTimer(int time, int totalTime, bool playing = false)
        {
            timerText.text = Utils.ConvertSecondsToTime(time);

            if (totalTime <= 0) return;

            var targetFill = Mathf.Clamp01((totalTime - time) / (float)totalTime);

            _fillTween?.Kill();
            _rotationTween?.Kill();

            var duration = Mathf.Approximately(targetFill, 1)
                ? minMaxAnimationDuration.x
                : minMaxAnimationDuration.y;

            _fillTween = DOTween
                .To(() => _currentFill,
                    x =>
                    {
                        _currentFill = x;

                        clockFillImage.fillAmount = x;

                        var rotation = -x * 360f;
                        knobPivot.localEulerAngles = new Vector3(0, 0, rotation);
                    },
                    targetFill,
                    duration)
                .SetEase(fillAnimationEase);
        }

        private void OnDestroy()
        {
            _fillTween?.Kill();
            _rotationTween?.Kill();
        }
    }
}