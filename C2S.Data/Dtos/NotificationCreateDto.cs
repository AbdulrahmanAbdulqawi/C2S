using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2S.Data.Dtos
{
    public class NotificationCreateDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } // "BookingConfirmation", "Reminder", etc.
        public bool IsRead { get; set; }
    }
}
