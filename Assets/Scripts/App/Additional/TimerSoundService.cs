using System;
using PT.Logic.Dependency.Signals;
using UnityEngine;
using Zenject;

namespace App.Additional
{
    public class TimerSoundService : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        
        [Inject] private readonly SignalBus _signalBus;

        private void Awake()
        {
            _signalBus.Subscribe<SessionFinishedSignal>(OnSessionFinished);
        }

        private void OnSessionFinished(SessionFinishedSignal signal)
        {
            audioSource.Play();
        }
        
        private void OnDestroy()
        {
            _signalBus.Unsubscribe<SessionFinishedSignal>(OnSessionFinished);
        }
    }
}