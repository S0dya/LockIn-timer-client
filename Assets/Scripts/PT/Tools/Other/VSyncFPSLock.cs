using UnityEngine;

namespace PT.Tools.Other
{
    public class VSyncFPSLock : MonoBehaviour
    {
        [SerializeField] int targetFPS = 60;
        [SerializeField] [Range(0,2)] int vSyncCount = 0;

        protected virtual void Start()
        {
            UpdateSettings();
        }	
        
        protected virtual void OnValidate()
        {
            UpdateSettings();
        }

        protected virtual void UpdateSettings()
        {
            QualitySettings.vSyncCount = vSyncCount;
            Application.targetFrameRate = targetFPS;
        }
    }
}