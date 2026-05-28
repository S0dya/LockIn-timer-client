using System;
using PT.Logic.Dependency.Signals;
using PT.Tools.Notifications;
using Zenject;

namespace App.Additional
{
    public class TimerNotificationsService : IInitializable, IDisposable
    {
        [Inject] private readonly NotificationsManager _notificationsManager;
        [Inject] private readonly SignalBus _signalBus;

        public void Initialize()
        {
            _signalBus.Subscribe<SessionStartedSignal>(ScheduleTimerFinishNotification);
        }
        
        private void ScheduleTimerFinishNotification(SessionStartedSignal signal)
        {
            _notificationsManager.Schedule(NotificationEnum.SessionFinished, new NotificationData("Session Completed", "Well Done! Time to rest", signal.Duration));
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<SessionStartedSignal>(ScheduleTimerFinishNotification);
        }
    }
}