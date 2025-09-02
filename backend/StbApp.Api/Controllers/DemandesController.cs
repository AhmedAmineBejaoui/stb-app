using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StbApp.Api.Data;
using StbApp.Api.Dtos.Demandes;
using StbApp.Api.Helpers;
using StbApp.Api.Models;

namespace StbApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DemandesController : ControllerBase
    {
        private readonly StbContext _db;
        public DemandesController(StbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DemandeDto>>> GetAll()
        {
            var isAdmin = User.IsInRole("Admin");
            var currentId = User.GetUserId();

            var query = _db.Demandes.AsNoTracking().AsQueryable();
            if (!isAdmin && currentId.HasValue)
            {
                query = query.Where(d => d.UserId == currentId.Value);
            }

            var result = await query
                .OrderByDescending(d => d.CreationDate)
                .Select(d => new DemandeDto
                {
                    Id = d.Id,
                    RequestType = d.RequestType,
                    RequestSubType = d.RequestSubType,
                    Description = d.Description,
                    CreationDate = d.CreationDate,
                    Status = d.Status,
                    UserId = d.UserId
                }).ToListAsync();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DemandeDto>> GetById(int id)
        {
            var demande = await _db.Demandes.AsNoTracking().Where(d => d.Id == id)
                .Select(d => new DemandeDto
                {
                    Id = d.Id,
                    RequestType = d.RequestType,
                    RequestSubType = d.RequestSubType,
                    Description = d.Description,
                    CreationDate = d.CreationDate,
                    Status = d.Status,
                    UserId = d.UserId
                }).FirstOrDefaultAsync();
            if (demande == null) return NotFound();

            var isAdmin = User.IsInRole("Admin");
            var currentId = User.GetUserId();
            if (!isAdmin && currentId != demande.UserId) return Forbid();

            return Ok(demande);
        }

        [HttpPost]
        public async Task<ActionResult<DemandeDto>> Create([FromBody] CreateDemandeDto dto)
        {
            var currentId = User.GetUserId();
            if (!currentId.HasValue) return Unauthorized();

            var demande = new Demande
            {
                RequestType = dto.RequestType,
                RequestSubType = dto.RequestSubType,
                Description = dto.Description,
                CreationDate = DateTime.Now,
                Status = "En attente",
                UserId = currentId.Value
            };
            _db.Demandes.Add(demande);
            await _db.SaveChangesAsync();

            var result = new DemandeDto
            {
                Id = demande.Id,
                RequestType = demande.RequestType,
                RequestSubType = demande.RequestSubType,
                Description = demande.Description,
                CreationDate = demande.CreationDate,
                Status = demande.Status,
                UserId = demande.UserId
            };
            return CreatedAtAction(nameof(GetById), new { id = demande.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDemandeDto dto)
        {
            var demande = await _db.Demandes.FindAsync(id);
            if (demande == null) return NotFound();

            var isAdmin = User.IsInRole("Admin");
            var currentId = User.GetUserId();
            if (!isAdmin && currentId != demande.UserId) return Forbid();

            if (dto.RequestType is not null) demande.RequestType = dto.RequestType;
            if (dto.RequestSubType is not null) demande.RequestSubType = dto.RequestSubType;
            if (dto.Description is not null) demande.Description = dto.Description;
            if (dto.Status is not null) demande.Status = dto.Status;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var demande = await _db.Demandes.FindAsync(id);
            if (demande == null) return NotFound();
            demande.Status = status;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var demande = await _db.Demandes.FindAsync(id);
            if (demande == null) return NotFound();
            _db.Demandes.Remove(demande);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}

