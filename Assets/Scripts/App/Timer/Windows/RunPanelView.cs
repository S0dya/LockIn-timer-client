using App.Timer.States;
using PT.Tools.Helper;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace App.Timer.Windows
{
    public abstract class RunPanelView : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI text;

        [Inject] protected AppState _appState;

        private readonly CompositeDisposable _compositeDisposable = new();
        
        private void Awake()
        {
            if (!text) text = GetComponent<TextMeshProUGUI>();
            
            _appState.RunState
                .Skip(1)
                .Subscribe(SetValue)
                .AddTo(_compositeDisposable);
        }
        
        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }
        
        protected abstract void SetValue(RunState runState);
    }
}