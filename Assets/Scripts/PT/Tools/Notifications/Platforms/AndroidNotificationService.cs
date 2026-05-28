using System;
using Unity.Notifications.Android;
using UnityEngine;

#if UNITY_ANDROID
namespace PT.Tools.Notifications.Platforms
{
    public class AndroidNotificationService : INotificationService
    {
        public void Schedule(int id, string title, string description, TimeSpan delay)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            using Unity.Notifications;
            
            var notification = new AndroidNotification
            {
                Title = title,
                Text = description,
                SmallIcon = "icon_0",
                LargeIcon = "app_icon",
                ShowTimestamp = true,
                FireTime = DateTime.Now.Add(delay)
            };

            AndroidNotificationCenter.SendNotification(notification, id);
            #endif
        }

        public void Cancel(int id)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            using Unity.Notifications;
            AndroidNotificationCenter.CancelScheduledNotification(id);
            #endif
        }

        public void CancelAll()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            using Unity.Notifications;
            AndroidNotificationCenter.CancelAllScheduledNotifications();
            #endif
        }
    }
}
#endif
