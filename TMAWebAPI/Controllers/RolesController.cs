using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMAWebAPI.Models;
using System.Data;
using TMAWebAPI.DTO;

namespace TMAWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly TMADbContext _context;
        private readonly ILogger<RolesController> _logger;

        public RolesController(TMADbContext context, ILogger<RolesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRole()
        {
            _logger.LogInformation("Received a Roles Get request");
            return await _context.Roles.ToListAsync();
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            _logger.LogInformation($"Received a Roles Get request by id: {id}");
            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, RoleDTO roleDTO)
        {
            Role roleTb = new Role();
            roleTb.RoleId = roleDTO.RoleId;
            roleTb.RoleName = roleDTO.RoleName;

            if (id != roleTb.RoleId)
            {
                return BadRequest();
            }

            _context.Entry(roleTb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Updated a Roles Put request by id: {id}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(RoleDTO roleDTO)
        {
            Role roleTb = new Role();
            roleTb.RoleId = roleDTO.RoleId;
            roleTb.RoleName = roleDTO.RoleName;

            _context.Roles.Add(roleTb);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Updated a Admin Post request by id: {roleTb.RoleId}");
            return CreatedAtAction("GetRole", new { id = roleTb.RoleId }, roleTb);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Deleted a roles data belongs to id: {id}");

            return NoContent();
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.RoleId == id);
        }
    }
}
