using C2S.Business.Interfaces;
using C2S.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace C2S.Business.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            notification.CreatedAt = DateTime.UtcNow;
            notification.IsRead = false;
            notification.Id = Guid.NewGuid();


            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return notification;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId)
        {
            if(!await _context.Notifications.AnyAsync()) 
                return Enumerable.Empty<Notification>();

            return await _context.Notifications
                                 .Include(n => n.User)
                                 .Where(n => n.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<Notification> MarkAsReadAsync(Guid notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);

            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return notification;
        }
    }
}
