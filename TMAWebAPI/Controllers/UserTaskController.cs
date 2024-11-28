using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using TMAWebAPI.Models;

namespace TMAWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTaskReportController : ControllerBase
    {

        private readonly TMADbContext _context;
        private readonly ILogger<UserTaskReportController> _logger;


        public UserTaskReportController(TMADbContext context, ILogger<UserTaskReportController> logger)
        {
            _context = context;
            _logger = logger;

        }

        // Endpoint to fetch user details by user ID
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
            _logger.LogInformation("Received a GetUserDetails request");
            return Ok(user);
        }

        // Endpoint to fetch all tasks for a specific user
        [HttpGet("TaskTbls/{userId}")]
        public async Task<IActionResult> GetUserTasks(int userId)
        {
            var tasks = await _context.TaskTbls
                .Where(t => t.AssignedToUserId == userId)
                .Select(t => new
                {
                    TaskId = t.TaskId,
                    TaskName = t.TaskName,
                    Status = t.Status
                })
                .ToListAsync();

            if (tasks == null || tasks.Count == 0)
            {
                return NotFound(new { Message = "No tasks found for this user." });
            }

            _logger.LogInformation("Received a GetUserTasks request");

            return Ok(tasks);
        }

        // Endpoint to generate and download user task report as CSV
        [HttpGet("TaskTbls/{userId}/downloadReport")]
        public async Task<IActionResult> DownloadUserReport(int userId)
        {

            // Fetch user tasks for the given userId
            var tasks = await _context.TaskTbls
                .Where(t => t.AssignedToUserId == userId)
                .Select(t => new
                {
                    t.TaskId,
                    t.TaskName,
                    t.Status,
                    t.TaskStartDate,
                    t.TaskEndDate
                })
                .ToListAsync();

            if (tasks == null || tasks.Count == 0)
            {
                return NotFound(new { Message = "No tasks found for this user." });
            }

            // Generate CSV content
            var csvContent = new StringBuilder();
            csvContent.AppendLine("Task ID,Task Name,Status,Start Date,End Date");

            foreach (var task in tasks)
            {
                csvContent.AppendLine($"{task.TaskId},{task.TaskName},{task.Status},{task.TaskStartDate:yyyy-MM-dd},{task.TaskEndDate:yyyy-MM-dd}");
            }

            var reportBytes = Encoding.UTF8.GetBytes(csvContent.ToString());
            var fileName = $"User_{userId}_Task_Report.csv";

            _logger.LogInformation("Received a DownloadUser request");
            return File(reportBytes, "text/csv", fileName);
        }

        [HttpGet("UserProductivityReport/{userId}")]
        public async Task<IActionResult> DownloadUserProductivityReport(int userId)
        {
            // Fetch user details
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

            // Fetch tasks for the user
            var tasks = await _context.TaskTbls
                .Where(t => t.AssignedToUserId == userId)
                .Select(t => new
                {
                    TaskName = t.TaskName,
                    Status = t.Status
                })
                .ToListAsync();

            if (tasks == null || tasks.Count == 0)
            {
                return NotFound(new { Message = "No tasks found for this user." });
            }

            // Calculate productivity
            int totalTasks = tasks.Count;
            int completedTasks = tasks.Count(t => t.Status == "Completed");
            int inProgressTasks = tasks.Count(t => t.Status == "In Progress");
            int notstarted = tasks.Count(t => t.Status == "Not Started");

            double productivity = (completedTasks * 100) + (inProgressTasks * 50);
            double productivityPercentage = totalTasks > 0 ? productivity / totalTasks : 0;

            // Generate txt content
            var tContent = new StringBuilder();
            tContent.AppendLine("User Productivity Report");
            tContent.AppendLine($"User Name: {user.UserName}");
            tContent.AppendLine($"Email: {user.Email}");
            tContent.AppendLine($"Total Tasks: {totalTasks}");
            tContent.AppendLine($"Completed Tasks: {completedTasks}");
            tContent.AppendLine($"In Progress Tasks: {inProgressTasks}");
            tContent.AppendLine($"Not Started Tasks: {notstarted}");
            tContent.AppendLine($"Productivity Percentage: {productivityPercentage:F2}%");
            tContent.AppendLine();
            tContent.AppendLine("Task Name,Status");

            foreach (var task in tasks)
            {
                tContent.AppendLine($"{task.TaskName},{task.Status}");
            }

            // Convert tcontent to bytes and return as file
            var tBytes = Encoding.UTF8.GetBytes(tContent.ToString());
            var fileName = $"User_{userId}_Productivity_Report.txt";

            _logger.LogInformation("Received a DownLoadUserReport request");
            return File(tBytes, "text/txt", fileName);
        }
    }

}