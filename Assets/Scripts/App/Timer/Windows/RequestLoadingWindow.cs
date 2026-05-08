using Cysharp.Threading.Tasks;
using PT.GameplayAdditional.Animations;
using PT.Tools.Windows;
using UnityEngine;
using UniRx;
using Zenject;

namespace App.Timer.Windows
{
    public class RequestLoadingWindow : WindowBase
    {
        [SerializeField] private GameObject rotatingObj;
        [Inject] private AppWindowsState _appWindowsState;
        
        private void Awake()
        {
            _appWindowsState.IsLoading
                .Subscribe(isLoading =>
                {
                    if (isLoading)
                    {
                        OpenAsync().Forget();
                    }
                    else
                    {
                        CloseAsync().Forget();
                    }
                })
                .AddTo(this);
        }

        protected override async UniTask OnOpen()
        {
            await base.OnOpen();
            
            rotatingObj.SetActive(true);
        }
        
        protected override async UniTask OnClose()
        {
            await base.OnClose();
            
            rotatingObj.SetActive(false);
        }
    }
}
