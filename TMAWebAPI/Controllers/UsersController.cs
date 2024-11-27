using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMAWebAPI.DTO;
using TMAWebAPI.Models;
using TMAWebAPI.DTO;

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
                RoleId = createUserDTO.RoleId  // Assuming UserRole is mapped as int
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

        [HttpGet("GetEmail/{email}")]
        public async Task<ActionResult<User>> GetUserByEmail(string email)
        {
            return Ok(await _context.Users.FirstAsync(users => users.Email.Equals(email)));
        }

        /// <summary>
        /// Allows updating a user's role by their UserId.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="updateRoleRequest">The request body containing the new RoleId.</param>
        /// <returns>
        /// A 200 OK response if the update is successful,
        /// a 400 Bad Request response if the input is invalid,
        /// or a 404 Not Found response if the user does not exist.
        /// </returns>
        [HttpPut("updateUserRole/{userId}")]
        public async Task<IActionResult> UpdateUserRole(int userId, [FromBody] UpdateUserRoleDTO updateRoleRequest)
        {
            // Validate the input
            if (updateRoleRequest == null || updateRoleRequest.RoleId <= 0)
            {
                return BadRequest(new { Message = "Invalid RoleId." });
            }

            // Step 1: Retrieve the user by userId
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound(new { Message = $"User with ID {userId} not found." });
            }

            // Step 2: Check if the RoleId is valid (exists in the Roles table)
            var roleExists = await _context.Roles.AnyAsync(r => r.RoleId == updateRoleRequest.RoleId);
            if (!roleExists)
            {
                return BadRequest(new { Message = "Invalid RoleId provided." });
            }

            // Step 3: Update the user's RoleId
            user.RoleId = updateRoleRequest.RoleId;

            // Step 4: Save changes to the database
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User role updated successfully." });
        }
    }
}