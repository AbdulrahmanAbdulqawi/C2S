using C2S.Business.Interfaces;
using C2S.Data.Dtos;
using C2S.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace C2S.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationCreateDto notificationDto)
        {
            var notification = new Notification
            {
                Message = notificationDto.Message,
                Type = notificationDto.Type,
                IsRead = notificationDto.IsRead,
                UserId = notificationDto.UserId
            };

            var createdNotification = await _notificationService.CreateNotificationAsync(notification);
            return CreatedAtAction(nameof(GetNotificationById), new { id = createdNotification.Id }, createdNotification);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(Guid id)
        {
            var notification = await _notificationService.MarkAsReadAsync(id);
            if (notification == null)
                return NotFound();
            return Ok(new {id = notification.Id, message = notification.Message });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetNotificationsByUserId(Guid userId)
        {
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);
            return Ok(notifications);
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var notification = await _notificationService.MarkAsReadAsync(id);
            if (notification == null)
                return NotFound();
            return Ok(notification);
        }
    }
}
