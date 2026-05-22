using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace App
{
    public class AppFocusSynchronization : MonoBehaviour
    {
        [Inject] private AppSynchronizationService _appSynchronizationService;
        
        private void OnApplicationPause(bool value)
        {
            if (!value)
                _appSynchronizationService.SynchronizeTimer().Forget();
        }
        private void OnApplicationFocus(bool value)
        {
            if (value)
                _appSynchronizationService.SynchronizeTimer().Forget();
        }
    }
}