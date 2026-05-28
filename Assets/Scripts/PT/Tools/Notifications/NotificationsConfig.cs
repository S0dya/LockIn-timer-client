using PT.Tools.Helper;
using UnityEngine;

namespace PT.Tools.Notifications
{
    [CreateAssetMenu(menuName = "Configs/NotificationsConfig", fileName = "NotificationsConfig")]
    public class NotificationsConfig : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField] private bool notificationsEnabled = true;
        [SerializeField] private bool requestPermissionOnInit = false;
        [SerializeField] private bool requestPermissionOnSchedule = true;
        [Space]
        [SerializeField] private SerializableKeyValue<NotificationEnum, NotificationData> notificationsKvpData;

        public bool NotificationsEnabled => notificationsEnabled;
        public bool RequestPermissionOnInit => requestPermissionOnInit;
        public bool RequestPermissionOnSchedule => requestPermissionOnSchedule;
        
        public SerializableKeyValue<NotificationEnum, NotificationData> NotificationsKvpData => notificationsKvpData;
    }
}
