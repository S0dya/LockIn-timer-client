using Cysharp.Threading.Tasks;
using DG.Tweening;
using PT.Tools.Windows;
using UnityEngine;

namespace App.Shared.Windows
{
    public class RequestLoadingWindow : WindowBase
    {
        [SerializeField] private GameObject rotatingObj;
        [SerializeField] private CanvasGroup canvasGroup;
        [Space]
        [SerializeField] private float fadeDelay = 0.05f;
        [SerializeField] private float fadeDuration = 0.15f;

        private Tween _fadeTween;

        protected override async UniTask OnOpen()
        {
            await base.OnOpen();

            _fadeTween?.Kill();

            rotatingObj.SetActive(true);

            canvasGroup.alpha = 0f;

            _fadeTween = DOVirtual
                .DelayedCall(fadeDelay, () =>
                {
                    _fadeTween = canvasGroup
                        .DOFade(1f, fadeDuration)
                        .SetEase(Ease.OutQuad);
                });
        }

        protected override async UniTask OnClose()
        {
            await base.OnClose();

            _fadeTween?.Kill();

            rotatingObj.SetActive(false);
        }

        private void OnDisable()
        {
            _fadeTween?.Kill();
        }
    }
}