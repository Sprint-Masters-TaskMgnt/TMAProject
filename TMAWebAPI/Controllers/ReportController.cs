using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text;
using TMAWebAPI.Models;

namespace TMAWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly TMADbContext _context;

        public ReportController(TMADbContext context)
        {
            _context = context;
        }

        // Generate detailed report for a specific project with all tasks
        [HttpGet("detailed-report/{projectId}")]
        public ActionResult GetDetailedReport(int projectId)
        {
            try
            {
                // Fetch project details and tasks using LINQ
                var projectDetails = (from p in _context.Projects
                                      where p.ProjectId == projectId
                                      select new
                                      {
                                          p.ProjectId,
                                          p.ProjectName,
                                          p.Description,
                                          p.StartDate,
                                          p.EndDate,
                                          Tasks = _context.TaskTbls
                                              .Where(t => t.ProjectId == p.ProjectId)
                                              .Select(t => new
                                              {
                                                  t.TaskId,
                                                  t.TaskName,
                                                  t.Description,
                                                  t.TaskStartDate,
                                                  t.TaskEndDate,
                                                  t.Priority,
                                                  t.Status
                                              }).ToList()
                                      }).FirstOrDefault();

                if (projectDetails == null)
                {
                    return NotFound("Project not found.");
                }

                // Build the response content
                var reportContent = new StringBuilder();
                reportContent.AppendLine("Project Report");
                reportContent.AppendLine("====================================");
                reportContent.AppendLine($"Project Name: {projectDetails.ProjectName}");
                reportContent.AppendLine($"Description: {projectDetails.Description}");
                reportContent.AppendLine($"Start Date: {projectDetails.StartDate:yyyy-MM-dd}");
                reportContent.AppendLine($"End Date: {projectDetails.EndDate:yyyy-MM-dd}");
                reportContent.AppendLine();
                reportContent.AppendLine("Tasks:");
                reportContent.AppendLine("Task Name,Description,Start Date,End Date,Priority,Status");

                foreach (var task in projectDetails.Tasks)
                {
                    reportContent.AppendLine($"{task.TaskName},{task.Description},{task.TaskStartDate:yyyy-MM-dd},{task.TaskEndDate:yyyy-MM-dd},{task.Priority},{task.Status}");
                }

                return Content(reportContent.ToString(), "text/plain");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Generate a consolidated report for all projects
        [HttpGet("consolidated-report")]
        public ActionResult GenerateConsolidatedReport()
        {
            try
            {
                var projectDetails = (from p in _context.Projects
                                      select new
                                      {
                                          p.ProjectId,
                                          p.ProjectName,
                                          p.Description,
                                          p.StartDate,
                                          p.EndDate,
                                          Tasks = _context.TaskTbls
                                              .Where(t => t.ProjectId == p.ProjectId)
                                              .Select(t => new
                                              {
                                                  t.TaskId,
                                                  t.TaskName,
                                                  t.Description,
                                                  t.TaskStartDate,
                                                  t.TaskEndDate,
                                                  t.Priority,
                                                  t.Status
                                              }).ToList()
                                      }).ToList();

                if (!projectDetails.Any())
                {
                    return NotFound("No projects found.");
                }

                var reportContent = new StringBuilder();
                reportContent.AppendLine("All Projects Report");
                reportContent.AppendLine("====================================");

                foreach (var project in projectDetails)
                {
                    reportContent.AppendLine($"Project Name: {project.ProjectName}");
                    reportContent.AppendLine($"Description: {project.Description}");
                    reportContent.AppendLine($"Start Date: {project.StartDate:yyyy-MM-dd}");
                    reportContent.AppendLine($"End Date: {project.EndDate:yyyy-MM-dd}");
                    reportContent.AppendLine("Tasks:");
                    reportContent.AppendLine("Task Name,Description,Start Date,End Date,Priority,Status");

                    foreach (var task in project.Tasks)
                    {
                        reportContent.AppendLine($"{task.TaskName},{task.Description},{task.TaskStartDate:yyyy-MM-dd},{task.TaskEndDate:yyyy-MM-dd},{task.Priority},{task.Status}");
                    }

                    reportContent.AppendLine(); // Separate projects
                }

                var reportBytes = Encoding.UTF8.GetBytes(reportContent.ToString());
                var fileName = "Consolidated_Projects_Report.csv";

                return File(reportBytes, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}