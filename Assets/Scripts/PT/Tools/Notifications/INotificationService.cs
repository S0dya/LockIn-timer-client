using System;

namespace PT.Tools.Notifications
{
    public interface INotificationService
    {
        void Schedule(int id, string title, string description, TimeSpan delay);
        void Cancel(int id);
        void CancelAll();
    }
}
