using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stb.Api.Data;
using Stb.Api.Models;

namespace Stb.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly StbContext _db;
    public NotificationsController(StbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Notification>>> GetAll([FromQuery] int? userId)
    {
        var query = _db.Notifications.AsNoTracking().AsQueryable();
        if (userId.HasValue)
            query = query.Where(n => n.UserId == userId.Value);
        return await query.OrderByDescending(n => n.CreationDate).ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Notification>> GetById(int id)
    {
        var notification = await _db.Notifications.FindAsync(id);
        return notification is null ? NotFound() : notification;
    }

    [HttpPost]
    public async Task<ActionResult<Notification>> Create(Notification notification)
    {
        _db.Notifications.Add(notification);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = notification.Id }, notification);
    }

    [HttpPatch("{id:int}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var notification = await _db.Notifications.FindAsync(id);
        if (notification is null) return NotFound();
        notification.IsRead = true;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var notification = await _db.Notifications.FindAsync(id);
        if (notification is null) return NotFound();
        _db.Notifications.Remove(notification);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

