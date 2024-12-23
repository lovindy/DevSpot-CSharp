using DevSpot.Models;
using DevSpot.Services;
using DevSpot.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevSpot.Controllers
{
    [Authorize]
    public class JobPostingsController : Controller
    {
        private readonly JobPostingService _jobPostingService;
        private readonly UserManager<IdentityUser> _userManager;

        public JobPostingsController(JobPostingService jobPostingService, UserManager<IdentityUser> userManager)
        {
            _jobPostingService = jobPostingService ?? throw new ArgumentNullException(nameof(jobPostingService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        // Action method for listing all job postings
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                var jobPostings = await _jobPostingService.GetAllJobPostingsAsync();
                return View(jobPostings);
            }
            catch (Exception ex)
            {
                // Log the exception (you could use a logging framework)
                Console.WriteLine($"Error fetching job postings: {ex.Message}");
                return View("Error", ex.Message);
            }
        }

        [Authorize(Roles = "Admin,Employer")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(JobPostingViewModel jobPostingViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var jobPosting = new JobPosting
                    {
                        Title = jobPostingViewModel.Title,
                        Description = jobPostingViewModel.Description,
                        Company = jobPostingViewModel.Company,
                        Location = jobPostingViewModel.Location,
                        UserId = _userManager.GetUserId(User)
                    };
                    await _jobPostingService.AddJobPostingAsync(jobPosting);
                    return RedirectToAction(nameof(Index));
                }
                return View(jobPostingViewModel);
            }
            catch (Exception ex)
            {
                // Log the exception (you could use a logging framework)
                Console.WriteLine($"Error creating job posting: {ex.Message}");
                return View("Error", ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Employer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var jobPosting = await _jobPostingService.GetJobPostingByIdAsync(id);
                if (jobPosting == null)
                {
                    return BadRequest(new { success = false, message = "Job posting not found" });
                }

                // Check if user is authorized to delete this posting
                var currentUserId = _userManager.GetUserId(User);
                if (!User.IsInRole("Admin") && jobPosting.UserId != currentUserId)
                {
                    return BadRequest(new { success = false, message = "Unauthorized to delete this posting" });
                }

                await _jobPostingService.DeleteJobPostingAsync(id);
                return Ok(new { success = true, message = "Job posting deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
