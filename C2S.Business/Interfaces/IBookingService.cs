using C2S.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2S.Business.Interfaces
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<Booking> GetBookingByIdAsync(Guid id);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking> UpdateBookingAsync(Guid id, Booking booking);
        Task<bool> DeleteBookingAsync(Guid id);
    }
}
