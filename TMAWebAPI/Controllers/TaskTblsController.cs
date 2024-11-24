using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMAWebAPI.DTO;
using TMAWebAPI.Models;
using TMAWebAPI.Services;

namespace TMAWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTblsController : ControllerBase
    {
        private readonly TMADbContext _context;
        private readonly IEmailService _emailService;

        public TaskTblsController(TMADbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: api/TaskTbls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskTbl>>> GetTaskTbls()
        {
            return await _context.TaskTbls.ToListAsync();
        }

        // GET: api/TaskTbls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskTbl>> GetTaskTbl(int id)
        {
            var taskTbl = await _context.TaskTbls.FindAsync(id);

            if (taskTbl == null)
            {
                return NotFound();
            }

            return taskTbl;
        }

        // PUT: api/TaskTbls/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskTbl(int id, TaskTblDTO taskDTO)
        {
            if (id != taskDTO.TaskId)
            {
                return BadRequest();
            }

            var taskTbl = new TaskTbl
            {
                TaskId = taskDTO.TaskId,
                TaskName = taskDTO.TaskName,
                AssignedToUserId = taskDTO.AssignedToUserId,
                ProjectId = taskDTO.ProjectId,
                Priority = taskDTO.Priority,
                Status = taskDTO.Status,
                TaskStartDate = taskDTO.TaskStartDate,
                TaskEndDate = taskDTO.TaskEndDate,
                Description = taskDTO.Description
            };

            _context.Entry(taskTbl).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskTblExists(id))
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

        // POST: api/TaskTbls
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=212375
        [HttpPost]
        public async Task<ActionResult<TaskTbl>> PostTaskTbl(TaskTblDTO taskDTO)
        {
            // Create a new TaskTbl entity from the DTO
            TaskTbl taskTbl = new TaskTbl
            {
                TaskName = taskDTO.TaskName,
                AssignedToUserId = taskDTO.AssignedToUserId,  // AssignedToUserId is an int
                ProjectId = taskDTO.ProjectId,
                Priority = taskDTO.Priority,
                Status = taskDTO.Status,
                TaskStartDate = taskDTO.TaskStartDate,
                TaskEndDate = taskDTO.TaskEndDate,
                Description = taskDTO.Description
            };

            // Add the new task to the context
            _context.TaskTbls.Add(taskTbl);

            try
            {
                // Save the changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Handle conflict if the TaskId already exists
                if (TaskTblExists(taskTbl.TaskId))
                {
                    return Conflict();
                }
                else
                {
                    throw;  // Re-throw if it's a different exception
                }
            }

            // Return the newly created TaskTbl, with a 201 status code and the created resource's URL
            return CreatedAtAction("GetTaskTbl", new { id = taskTbl.TaskId }, taskTbl);
        }


        // DELETE: api/TaskTbls/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskTbl(int id)
        {
            var taskTbl = await _context.TaskTbls.FindAsync(id);
            if (taskTbl == null)
            {
                return NotFound();
            }

            _context.TaskTbls.Remove(taskTbl);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskTblExists(int id)
        {
            return _context.TaskTbls.Any(e => e.TaskId == id);
        }

        // PATCH: api/TaskTbls/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] TaskStatusUpdateDTO request)
        {
            if (string.IsNullOrEmpty(request?.Status))
            {
                return BadRequest("Status is required.");
            }

            var task = await _context.TaskTbls.FindAsync(id);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            // Update task details
            task.Status = request.Status;
            task.Priority = request.Priority;
            task.TaskEndDate = (DateTime)request.TaskEndDate;

            await _context.SaveChangesAsync();

            // Fetch user email by userId
            string userEmail;
            try
            {
                userEmail = await _emailService.GetUserEmailByIdAsync(task.AssignedToUserId);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }

            // If status is "Pending", send reminder email
            if (task.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
            {
                var subject = $"Reminder: Task '{task.TaskName}' is Pending";
                var body = $"Dear User,\n\nTask '{task.TaskName}' is pending:\n" +
                           $"- Priority: {task.Priority}\n- Due Date: {task.TaskEndDate:yyyy-MM-dd}\n\n" +
                           "Please complete the task promptly.\n\nTask Management System";

                await _emailService.SendEmailAsync(userEmail, subject, body);
            }

            // Notify admin
            var adminEmail = "admin@example.com";
            var adminSubject = $"Task '{task.TaskName}' Status Updated";
            var adminBody = $"Task '{task.TaskName}' updated by {userEmail}:\n" +
                            $"- Status: {task.Status}\n- Priority: {task.Priority}\n- End Date: {task.TaskEndDate:yyyy-MM-dd}";

            await _emailService.SendEmailAsync(adminEmail, adminSubject, adminBody);

            return NoContent();
        }

        // GET: api/TaskTbls/getProjectDetails/{projectId}
        [HttpGet("getProjectDetails/{projectId}")]
        public async Task<IActionResult> GetProjectDetails(int projectId)
        {
            var project = await _context.Projects
                .Where(p => p.ProjectId == projectId)
                .Select(p => new
                {
                    p.ProjectId,
                    p.ProjectName,
                    p.Description,
                    p.StartDate,
                    p.EndDate,
                    Tasks = _context.TaskTbls
                        .Where(t => t.ProjectId == projectId)
                        .Select(t => new
                        {
                            t.TaskId,
                            t.TaskName,
                            t.AssignedToUserId,
                            t.Description,
                            t.TaskStartDate,
                            t.TaskEndDate,
                            t.Priority,
                            t.Status
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (project == null)
            {
                return NotFound(new { Message = $"No project found with ID: {projectId}" });
            }

            return Ok(project);
        }

        // GET: api/TaskTbls/downloadProjectReport/{projectId}
        [HttpGet("downloadProjectReport/{projectId}")]
        public async Task<IActionResult> DownloadProjectReport(int projectId)
        {
            var project = await _context.Projects
                .Where(p => p.ProjectId == projectId)
                .Select(p => new
                {
                    p.ProjectName,
                    p.Description,
                    p.StartDate,
                    p.EndDate,
                    Tasks = _context.TaskTbls
                        .Where(t => t.ProjectId == projectId)
                        .Select(t => new
                        {
                            t.TaskName,
                            t.Description,
                            t.AssignedToUserId,
                            t.TaskStartDate,
                            t.TaskEndDate,
                            t.Priority,
                            t.Status
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (project == null)
            {
                return NotFound($"No project found with ID: {projectId}");
            }

            var reportContent = new StringBuilder();
            reportContent.AppendLine($"Project Name: {project.ProjectName}");
            reportContent.AppendLine($"Description: {project.Description}");
            reportContent.AppendLine("Tasks:");
            reportContent.AppendLine("Task Name,Description,Start Date,End Date,Priority,Status");

            foreach (var task in project.Tasks)
            {
                reportContent.AppendLine($"{task.TaskName},{task.Description},{task.TaskStartDate:yyyy-MM-dd},{task.TaskEndDate:yyyy-MM-dd},{task.Priority},{task.Status}");
            }

            var reportBytes = Encoding.UTF8.GetBytes(reportContent.ToString());
            var fileName = $"Project_{projectId}_Report.csv";

            return File(reportBytes, "text/csv", fileName);
        }

        // GET: api/TaskTbls/Users/{userId}
        [HttpGet("Users/{userId}")]
        public async Task<IActionResult> GetUserDetails(int userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new
                {
                    UserId = u.Id,
                    UserName = u.UserName,
                    Email = u.Email
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { Message = $"User with ID {userId} not found." });
            }

            return Ok(user);
        }

        // GET: api/TaskTbls/TaskTbls/{userId}
        [HttpGet("TaskTbls/{userId}")]
        public async Task<IActionResult> GetUserTasks(int userId)
        {
            var tasks = await _context.TaskTbls
                .Where(t => t.AssignedToUserId == userId)
                .Select(t => new
                {
                    t.TaskId,
                    t.TaskName,
                    t.Status
                })
                .ToListAsync();

            if (!tasks.Any())
            {
                return NotFound(new { Message = $"No tasks found for user ID: {userId}" });
            }

            return Ok(tasks);
        }
    }
}