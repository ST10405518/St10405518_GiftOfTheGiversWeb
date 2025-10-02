using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using St10405518_GiftOfTheGiversWeb.Data;
using St10405518_GiftOfTheGiversWeb.Models;
using St10405518_GiftOfTheGiversWeb.Models.ViewModels;
using System.Data;

namespace St10405518_GiftOfTheGiversWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminVolunteerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminVolunteerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var dashboard = new AdminDashboardVM
            {
                TotalVolunteers = _context.Volunteers.Count(),
                ActiveTasks = _context.VolunteerTasks.Count(t => t.Status == "Open" || t.Status == "In Progress"),
                CompletedTasks = _context.VolunteerTasks.Count(t => t.Status == "Completed"),
                RecentRegistrations = _context.Volunteers
                    .OrderByDescending(v => v.RegistrationDate)
                    .Take(5)
                    .ToList()
            };
            return View(dashboard);
        }

        public async Task<IActionResult> Volunteers()
        {
            var volunteers = await _context.Volunteers.ToListAsync();
            return View(volunteers);
        }

        public async Task<IActionResult> VolunteerDetails(int id)
        {
            var volunteer = await _context.Volunteers
                .Include(v => v.Tasks!)
                .ThenInclude(ta => ta.VolunteerTask)
                .FirstOrDefaultAsync(v => v.VolunteerID == id);

            if (volunteer == null)
            {
                return NotFound();
            }

            return View(volunteer);
        }

        public async Task<IActionResult> Tasks()
        {
            var tasks = await _context.VolunteerTasks.ToListAsync();
            return View(tasks);
        }

        public IActionResult CreateTask()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask(VolunteerTask task)
        {
            if (ModelState.IsValid)
            {
                _context.VolunteerTasks.Add(task);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Task created successfully!";
                return RedirectToAction(nameof(Tasks));
            }
            return View(task);
        }

        public async Task<IActionResult> AssignVolunteer(int taskId)
        {
            var task = await _context.VolunteerTasks.FindAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }

            var availableVolunteers = await _context.Volunteers
                .Where(v => v.Status == "Active")
                .ToListAsync();

            var viewModel = new AdminAssignmentVM
            {
                Task = task,
                AvailableVolunteers = availableVolunteers
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignVolunteer(int taskId, int volunteerId)
        {
            var existingAssignment = await _context.TaskAssignments
                .FirstOrDefaultAsync(ta => ta.VolunteerID == volunteerId && ta.TaskID == taskId);

            if (existingAssignment != null)
            {
                TempData["ErrorMessage"] = "This volunteer is already assigned to this task.";
                return RedirectToAction(nameof(AssignVolunteer), new { taskId });
            }

            var assignment = new TaskAssignment
            {
                VolunteerID = volunteerId,
                TaskID = taskId,
                AssignmentDate = DateTime.Now,
                Status = "Assigned"
            };

            _context.TaskAssignments.Add(assignment);

            var task = await _context.VolunteerTasks.FindAsync(taskId);
            if (task != null)
            {
                task.CurrentVolunteers++;
                _context.VolunteerTasks.Update(task);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Volunteer assigned successfully!";
            return RedirectToAction(nameof(TaskDetails), new { id = taskId });
        }

        public async Task<IActionResult> TaskDetails(int id)
        {
            var task = await _context.VolunteerTasks
                .Include(t => t.Assignments!)
                .ThenInclude(a => a.Volunteer)
                .FirstOrDefaultAsync(t => t.TaskID == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                var existingVolunteer = await _context.Volunteers
                    .FirstOrDefaultAsync(v => v.Email == volunteer.Email);

                if (existingVolunteer != null)
                {
                    ModelState.AddModelError("Email", "A volunteer with this email already exists.");
                    return View(volunteer);
                }

                volunteer.RegistrationDate = DateTime.Now;
                volunteer.Status = "Active";
                _context.Volunteers.Add(volunteer);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thank you for registering as a volunteer! We will contact you soon.";
                return RedirectToAction(nameof(RegisterSuccess));
            }
            return View(volunteer);
        }

        [AllowAnonymous]
        public IActionResult RegisterSuccess()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTaskStatus(int taskId, string status)
        {
            var task = await _context.VolunteerTasks.FindAsync(taskId);
            if (task != null)
            {
                task.Status = status;
                _context.VolunteerTasks.Update(task);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Task status updated successfully!";
            }
            return RedirectToAction(nameof(TaskDetails), new { id = taskId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAssignmentStatus(int assignmentId, string status, decimal? hoursWorked = null)
        {
            var assignment = await _context.TaskAssignments.FindAsync(assignmentId);
            if (assignment != null)
            {
                assignment.Status = status;
                if (hoursWorked.HasValue)
                {
                    assignment.HoursWorked = hoursWorked;
                }
                _context.TaskAssignments.Update(assignment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Assignment updated successfully!";
            }
            return RedirectToAction(nameof(TaskDetails), new { id = assignment.TaskID });
        }
    }
}