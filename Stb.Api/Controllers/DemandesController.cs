using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stb.Api.Data;
using Stb.Api.Models;

namespace Stb.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DemandesController : ControllerBase
{
    private readonly StbContext _db;
    public DemandesController(StbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Demande>>> GetAll()
        => await _db.Demandes.AsNoTracking().Include(d => d.User).ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Demande>> GetById(int id)
    {
        var demande = await _db.Demandes
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.Id == id);
        return demande is null ? NotFound() : demande;
    }

    [HttpGet("by-user/{userId:int}")]
    public async Task<ActionResult<IEnumerable<Demande>>> GetByUser(int userId)
        => await _db.Demandes.AsNoTracking()
            .Where(d => d.UserId == userId)
            .OrderByDescending(d => d.CreationDate)
            .ToListAsync();

    [HttpPost]
    public async Task<ActionResult<Demande>> Create(Demande demande)
    {
        if (string.IsNullOrWhiteSpace(demande.Status))
            demande.Status = "En attente";
        _db.Demandes.Add(demande);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = demande.Id }, demande);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Demande update)
    {
        if (id != update.Id) return BadRequest();
        var exists = await _db.Demandes.AnyAsync(d => d.Id == id);
        if (!exists) return NotFound();
        _db.Entry(update).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
    {
        var demande = await _db.Demandes.FindAsync(id);
        if (demande is null) return NotFound();
        demande.Status = status;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var demande = await _db.Demandes.FindAsync(id);
        if (demande is null) return NotFound();
        _db.Demandes.Remove(demande);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

