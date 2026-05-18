using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PT.UI.Buttons
{
    public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private Image progressImage;
        [SerializeField] private RectTransform targetTransform;
        [Space]
        [SerializeField] private float holdDuration = 1f;
        [SerializeField] private float returnDuration = 0.2f;
        [Space]
        [SerializeField] private float pressedScale = 0.92f;
        [SerializeField] private float completedScale = 1.08f;
        [Space]
        [SerializeField] private Ease holdEase = Ease.Linear;
        [SerializeField] private Ease returnEase = Ease.OutQuad;

        public event Action OnCompleted;

        private bool _holding;
        private bool _completed;

        private float _progress;

        private Tween _progressTween;
        private Tween _scaleTween;

        private Vector3 _baseScale;

        private void Awake()
        {
            if (!targetTransform) targetTransform = transform as RectTransform;

            _baseScale = targetTransform.localScale;

            ResetVisualsImmediate();
        }

        private void OnEnable()
        {
            ResetVisualsImmediate();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_completed) return;

            _holding = true;

            _progressTween?.Kill();
            _scaleTween?.Kill();

            _scaleTween = targetTransform
                .DOScale(_baseScale * pressedScale, 0.12f)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true);

            var durationLeft = holdDuration * (1f - _progress);

            _progressTween = DOTween
                .To(() => _progress,
                    x =>
                    {
                        _progress = x;
                        progressImage.fillAmount = x;

                        var scale = Mathf.Lerp(pressedScale, completedScale, x);
                        targetTransform.localScale = _baseScale * scale;
                    },
                    1f,
                    durationLeft)
                .SetEase(holdEase)
                .SetUpdate(true)
                .OnComplete(Complete);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            StopHolding();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopHolding();
        }

        private void StopHolding()
        {
            if (!_holding || _completed) return;

            _holding = false;

            _progressTween?.Kill();
            _scaleTween?.Kill();

            var duration = returnDuration * _progress;

            _progressTween = DOTween
                .To(() => _progress,
                    x =>
                    {
                        _progress = x;
                        progressImage.fillAmount = x;

                        var scale = Mathf.Lerp(pressedScale, completedScale, x);
                        targetTransform.localScale = _baseScale * Mathf.Max(scale, 1f);
                    },
                    0f,
                    duration)
                .SetEase(returnEase)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    targetTransform.localScale = _baseScale;
                });
        }

        private void Complete()
        {
            if (_completed) return;

            _completed = true;
            _holding = false;

            _progress = 1f;
            progressImage.fillAmount = 1f;

            _scaleTween?.Kill();

            _scaleTween = DOTween.Sequence()
                .Append(targetTransform.DOScale(_baseScale * completedScale, 0.08f))
                .Append(targetTransform.DOScale(_baseScale, 0.12f))
                .SetEase(Ease.OutQuad)
                .SetUpdate(true);

            OnCompleted?.Invoke();
        }

        private void ResetVisualsImmediate()
        {
            _holding = false;
            _completed = false;

            _progress = 0f;

            _progressTween?.Kill();
            _scaleTween?.Kill();

            progressImage.fillAmount = 0f;

            if (targetTransform) targetTransform.localScale = _baseScale;
        }

        private void OnDestroy()
        {
            _progressTween?.Kill();
            _scaleTween?.Kill();
        }
    }
}