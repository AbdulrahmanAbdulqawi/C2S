using C2S.Data.Enumrations;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace C2S.Data.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; } // "Customer" or "Provider"
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<Service> Services { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Booking> ProvidedBookings { get; set; }  // As Provider
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
