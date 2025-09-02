using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StbApp.Api.Data;
using StbApp.Api.Dtos.Notifications;
using StbApp.Api.Helpers;
using StbApp.Api.Models;

namespace StbApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly StbContext _db;
        public NotificationsController(StbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetMyNotifications()
        {
            var userId = User.GetUserId();
            if (!userId.HasValue) return Unauthorized();

            var result = await _db.Notifications.AsNoTracking()
                .Where(n => n.UserId == userId.Value)
                .OrderByDescending(n => n.CreationDate)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Message = n.Message,
                    CreationDate = n.CreationDate,
                    IsRead = n.IsRead,
                    UserId = n.UserId,
                    DemandeId = n.DemandeId
                }).ToListAsync();
            return Ok(result);
        }

        [HttpPut("{id:int}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = User.GetUserId();
            if (!userId.HasValue) return Unauthorized();

            var notif = await _db.Notifications.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId.Value);
            if (notif == null) return NotFound();
            notif.IsRead = true;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<NotificationDto>> Create([FromBody] CreateNotificationDto dto)
        {
            var existsUser = await _db.Users.AnyAsync(u => u.Id == dto.UserId);
            var existsDemande = await _db.Demandes.AnyAsync(d => d.Id == dto.DemandeId);
            if (!existsUser || !existsDemande) return BadRequest("Utilisateur ou Demande introuvable");

            var notif = new Notification
            {
                Message = dto.Message,
                CreationDate = DateTime.Now,
                IsRead = false,
                UserId = dto.UserId,
                DemandeId = dto.DemandeId
            };
            _db.Notifications.Add(notif);
            await _db.SaveChangesAsync();
            var result = new NotificationDto
            {
                Id = notif.Id,
                Message = notif.Message,
                CreationDate = notif.CreationDate,
                IsRead = notif.IsRead,
                UserId = notif.UserId,
                DemandeId = notif.DemandeId
            };
            return CreatedAtAction(nameof(GetMyNotifications), new { id = notif.Id }, result);
        }
    }
}

