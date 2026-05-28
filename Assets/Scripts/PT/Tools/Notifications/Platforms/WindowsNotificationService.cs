using System;
using System.Runtime.InteropServices;
using UnityEngine;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
namespace PT.Tools.Notifications.Platforms
{
    public class WindowsNotificationService : INotificationService
    {
        public void Schedule(int id, string title, string description, TimeSpan delay)
        {
            
        }

        public void Cancel(int id)
        {
            
        }

        public void CancelAll()
        {
            
        }

        private void ShowToast(string title, string description)
        {
            
        }
    }
}
#endif
