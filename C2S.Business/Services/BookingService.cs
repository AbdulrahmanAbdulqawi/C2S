using C2S.Business.Interfaces;
using C2S.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace C2S.Business.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {

            booking.Id = Guid.NewGuid();
            booking.CreatedAt = DateTime.UtcNow;
            booking.UpdatedAt = DateTime.UtcNow;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking> GetBookingByIdAsync(Guid id)
        {
            if (await _context.Bookings.AnyAsync())
            {
                return await _context.Bookings
                 .Include(b => b.Customer)
                 .Include(b => b.Service)
                 .Include(b => b.Provider)
                 .FirstOrDefaultAsync(b => b.Id == id);
            }
            return null;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Service)
                .Include(b => b.Provider)
                .ToListAsync();
        }

        public async Task<Booking> UpdateBookingAsync(Guid id, Booking booking)
        {
            var existingBooking = await _context.Bookings.FindAsync(id);
            if (existingBooking == null)
            {
                return null;
            }

            existingBooking.CustomerId = booking.CustomerId;
            existingBooking.ServiceId = booking.ServiceId;
            existingBooking.ProviderId = booking.ProviderId;
            existingBooking.Date = booking.Date;
            existingBooking.Time = booking.Time;
            existingBooking.Status = booking.Status;
            existingBooking.UpdatedAt = DateTime.UtcNow;

            _context.Bookings.Update(existingBooking);
            await _context.SaveChangesAsync();
            return existingBooking;
        }

        public async Task<bool> DeleteBookingAsync(Guid id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return false;
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
