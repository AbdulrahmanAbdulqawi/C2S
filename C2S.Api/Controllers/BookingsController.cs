using C2S.Business.Interfaces;
using C2S.Data.Dtos;
using C2S.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace C2S.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(Guid id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingCreateDto bookingDto)
        {
            var bookingExists = await _bookingService.GetBookingByIdAsync(bookingDto.Id);
            if (bookingExists != null) return Conflict();

            var booking = new Booking
            {
                ServiceId = bookingDto.ServiceId,
                Status = bookingDto.Status,
                CustomerId = bookingDto.CustomerId,
                Date = bookingDto.Date,
                Id = bookingDto.Id,
                ProviderId = bookingDto.ProviderId,
                Time = bookingDto.Time
            };

            var createdBooking = await _bookingService.CreateBookingAsync(booking);
            return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.Id }, createdBooking);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(Guid id, [FromBody] BookingUpdateDto bookingDto)
        {
            var bookingExists = _bookingService.GetBookingByIdAsync(id);
            if (bookingExists == null) return NotFound();

            var booking = new Booking
            {
                Status = bookingDto.Status,
                Date = bookingDto.Date,
                Id = bookingDto.Id,
                Time = bookingDto.Time
            };

            var updatedBooking = await _bookingService.UpdateBookingAsync(id, booking);
            if (updatedBooking == null)
            {
                return NotFound();
            }

            return Ok(updatedBooking);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            var result = await _bookingService.DeleteBookingAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
