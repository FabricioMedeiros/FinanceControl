using FinanceControl.Application.Notifications;

namespace FinanceControl.Application.Interfaces
{
    public interface INotificator
    {
        List<Notification> GetNotifications();
        void AddNotification(Notification notification);
        bool HasNotification();
    }
}
