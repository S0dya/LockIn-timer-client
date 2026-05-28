using System;
using PT.UI.Buttons;
using TMPro;
using UniRx;
using UnityEngine;

namespace App.Features.Settings
{
    public class TimerSettingView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private BasicButton increaseButton;
        [SerializeField] private CanvasGroup increaseCg;
        [SerializeField] private BasicButton decreaseButton;
        [SerializeField] private CanvasGroup decreaseCg;
            
        [HideInInspector] public ReactiveProperty<int> CurrentIndex;

        private CompositeDisposable _disposable = new();
            
        public void Init(int[] values, int startIndex, Func<int, string> conversionFunc = null)
        {
            CurrentIndex = new ReactiveProperty<int>(startIndex);
                
            CurrentIndex.Subscribe(index =>
            {
                amountText.text = conversionFunc != null ? 
                    conversionFunc(values[index]) : 
                    values[index].ToString();
                decreaseCg.interactable = index > 0;
                increaseCg.interactable = index < values.Length - 1;
            }).AddTo(_disposable);
                
            decreaseButton.SetOnClick(() => {
                if (CurrentIndex.Value > 0) CurrentIndex.Value--;
            });
                
            increaseButton.SetOnClick(() => {
                if (CurrentIndex.Value < values.Length - 1) CurrentIndex.Value++;
            });
        }
        
        private void OnDestroy() => _disposable.Dispose();
    }
}