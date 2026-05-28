using System.Collections.Generic;
using PT.Logic.Save;
using PT.Tools.Debugging;
using Zenject;

#if UNITY_ANDROID && !UNITY_EDITOR
using Unity.Notifications.Android;
#endif

namespace PT.Tools.Notifications
{
    public class NotificationsManager 
    {
        [Inject] private readonly INotificationService _notificationService;
        [Inject] private readonly NotificationsConfig _config;

        private readonly Dictionary<NotificationEnum, int> _notifications = new();
        
        private int _nextId = 1;
        
        public void Init()
        {
            if (_config.RequestPermissionOnInit && !(bool)GameDataRegistry.Get(GameDataKey.NotificationsRequested)) RequestNotificationPermission();
        }
        
        public void Schedule(NotificationEnum notificationEnum)
        {
            if (_config.NotificationsKvpData.Dictionary.ContainsKey(notificationEnum))
            {
                Schedule(notificationEnum, _config.NotificationsKvpData.Dictionary[notificationEnum]);
            }
            else
            {
                DebugManager.Log(DebugCategory.Notifications, $"Notification data for {notificationEnum} not found");
            }
        }
        
        public void Schedule(NotificationEnum notificationEnum, NotificationData data)
        {
            if (_config.RequestPermissionOnSchedule && !(bool)GameDataRegistry.Get(GameDataKey.NotificationsRequested)) RequestNotificationPermission();
            
            if (!_config.NotificationsEnabled) return;

            if (_notifications.ContainsKey(notificationEnum))
            {
                Cancel(notificationEnum);
            }

            int id = _nextId++;
            
            _notifications[notificationEnum] = id;
            
            _notificationService.Schedule(id, 
                data.Title, data.Description, 
                data.Delay);
            
            DebugManager.Log(DebugCategory.Notifications, $"Notification {notificationEnum} scheduled with : " +
                                                          $"id {id} " +
                                                          $"delay {data.Delay}" +
                                                          $"title {data.Title}" +
                                                          $"description {data.Description}");
        }

        public void Cancel(NotificationEnum notificationEnum)
        {
            if (_notifications.ContainsKey(notificationEnum))
            {
                _notificationService.Cancel(_notifications[notificationEnum]);
                _notifications.Remove(notificationEnum);
            }
            else
            {
                DebugManager.Log(DebugCategory.Notifications, $"Notification {notificationEnum} not found, so cant be cancelled");
            }
        }

        public void CancelAll()
        {
            DebugManager.Log(DebugCategory.Notifications, $"All notifications cancelled");
            
            _notificationService.CancelAll();
            _notifications.Clear();
        }
        
        
        private void RequestNotificationPermission()
        {
            if ((bool)GameDataRegistry.Get(GameDataKey.NotificationsRequested)) return;
            GameDataRegistry.Set(GameDataKey.NotificationsRequested, true);
            
#if UNITY_ANDROID && !UNITY_EDITOR
            if (AndroidNotificationCenter.UserPermissionToPost == PermissionStatus.Allowed)
            {
                DebugManager.Log(DebugCategory.Notifications, "Notifications permission already granted");
                return;
            }

            AndroidNotificationCenter.RequestPermission();
            
            DebugManager.Log(DebugCategory.Notifications, "Requesting notifications permission");
#endif
        }
    }
}
