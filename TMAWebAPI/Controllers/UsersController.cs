using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMAWebAPI.DTO;
using TMAWebAPI.Models;

using Microsoft.EntityFrameworkCore;

namespace TMAWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TMADbContext _context;
     
            public UsersController(TMADbContext context)
            {
                _context = context;
            }

            // GET: api/User
            [HttpGet]
            public async Task<IActionResult> GetAllUsers()
            {
                var users = await _context.Users
                    .Select(u => new
                    {
                        u.Id,
                        u.Email,
                        u.PhoneNumber,
                        u.UserName,
                        u.RoleId
                    })
                    .ToListAsync();

                return Ok(users);
            }

            // GET: api/User/{id}
            [HttpGet("{id}")]
            public async Task<IActionResult> GetUser(int id)
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound(new { Message = $"User with ID {id} not found." });
                }

                return Ok(new
                {
                    user.Id,
                    user.Email,
                    user.PhoneNumber,
                    user.UserName,
                    user.RoleId
                });
            }

            // POST: api/User
            [HttpPost]
            public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO createUserDTO)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Create a new User entity from the DTO
                var user = new User
                {
                    Email = createUserDTO.Email,
                    PhoneNumber = createUserDTO.PhoneNumber,
                    PasswordHash = createUserDTO.PasswordHash,
                    UserName = createUserDTO.UserName,
                    RoleId = createUserDTO.RoleId // Assuming UserRole is mapped as int
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }

            // PUT: api/User/{id}
            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateUser(int id, [FromBody] CreateUserDTO updateUserDTO)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { Message = $"User with ID {id} not found." });
                }

                // Update properties
                user.Email = updateUserDTO.Email;
                user.PhoneNumber = updateUserDTO.PhoneNumber;
                user.PasswordHash = updateUserDTO.PasswordHash;
                user.UserName = updateUserDTO.UserName;
                user.RoleId = updateUserDTO.RoleId; // Assuming UserRole is mapped as int

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            // DELETE: api/User/{id}
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteUser(int id)
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { Message = $"User with ID {id} not found." });
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }

