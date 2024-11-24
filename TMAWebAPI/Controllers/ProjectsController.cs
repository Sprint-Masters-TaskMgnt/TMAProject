using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMAWebAPI.Models;
using TMAWebAPI.DTO;

namespace TMAWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly TMADbContext _context;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(TMADbContext context, ILogger<ProjectsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProject()
        {
            _logger.LogInformation("Received a Projects Get request");
            return await _context.Projects.ToListAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            _logger.LogInformation($"Received a Projects Get request by id: {id}");
            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, ProjectDTO projectDTO)
        {
            Project projectTb = new Project();
            projectTb.ProjectId = projectDTO.ProjectId;
            projectTb.ProjectName = projectDTO.ProjectName;
            projectTb.StartDate = projectDTO.StartDate;
            projectTb.EndDate = projectDTO.EndDate;
            projectTb.Status = projectDTO.Status;
            projectTb.Description = projectDTO.Description;

            if (id != projectTb.ProjectId)
            {
                return BadRequest();
            }

            _context.Entry(projectTb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Updated a projects Put request by id: {id}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(ProjectDTO projectDTO)
        {
            Project projectTb = new Project();
            projectTb.ProjectId = projectDTO.ProjectId;
            projectTb.ProjectName = projectDTO.ProjectName;
            projectTb.StartDate = projectDTO.StartDate;
            projectTb.EndDate = projectDTO.EndDate;
            projectTb.Status = projectDTO.Status;
            projectTb.Description = projectDTO.Description;

            _context.Projects.Add(projectTb);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Updated a project Post request by id: {projectTb.ProjectId}");
            return CreatedAtAction("Getproject", new { id = projectTb.ProjectId }, projectTb);
        }

        // DELETE: api/projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Deleted a Projects data belongs to id: {id}");

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }
    }
}