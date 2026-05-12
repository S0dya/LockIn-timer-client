using Cysharp.Threading.Tasks;
using PT.GameplayAdditional.Animations;
using PT.Tools.Windows;
using UnityEngine;

namespace App.Timer.Windows
{
    public class RequestLoadingWindow : WindowBase
    {
        [SerializeField] private GameObject rotatingObj;

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
