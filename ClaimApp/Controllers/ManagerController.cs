using Microsoft.AspNetCore.Mvc;
using ClaimApp.Models;
using ClaimApp.Data;
using System.Linq;
using Microsoft.AspNetCore.Http; // For session management
using System.Security.Claims; // For user claims
using Microsoft.EntityFrameworkCore; // For Include

namespace ClaimApp.Controllers
{
    public class ManagerController : Controller
    {
        private readonly ClaimAppContext _context;

        public ManagerController(ClaimAppContext context)
        {
            _context = context;
        }
   
        
        // GET: Manager/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Manager/Login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var manager = _context.Managers
                    .FirstOrDefault(m => m.Email == model.Email && m.Password == model.Password);

                if (manager != null)
                {
                    // Set session or claims for the logged-in manager
                    HttpContext.Session.SetString("ManagerId", manager.ManagerId.ToString());
                    HttpContext.Session.SetString("ManagerName", manager.FirstName);

                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid login credentials.";
                }
            }
            return View(model);
        }

        // GET: Manager/Dashboard
        public IActionResult Dashboard()
        {
            // Check if the manager is logged in via session
            var managerId = HttpContext.Session.GetString("ManagerId");
            if (string.IsNullOrEmpty(managerId))
            {
                return RedirectToAction("Login");
            }

            // Retrieve manager name from session
            var managerName = HttpContext.Session.GetString("ManagerName");

            // Create the view model with pending claims data
            var dashboardViewModel = new ManagerDashboardViewModel
            {
                ManagerName = managerName,
                TotalClaims = _context.Claims.Count(),
                ApprovedClaims = _context.Claims.Count(c => c.Status == "Approved"),
                PendingClaims = _context.Claims.Count(c => c.Status == "Pending"),
                RejectedClaims = _context.Claims.Count(c => c.Status == "Rejected"),

                // Pending claims with related lecturer information
                PendingClaimsList = _context.Claims
                    .Where(c => c.Status == "Pending")
                    .Include(c => c.Lecturer)  // Include the related Lecturer entity
                    .Select(c => new PendingClaimViewModel
                    {
                        Id = c.Id,
                        LecturerName = c.Lecturer.FirstName + " " + c.Lecturer.LastName,
                        ModuleName = c.ModuleName,
                        HoursWorked = (int)c.HoursWorked,  
                        TotalAmount = (decimal)c.TotalAmount,  
                        DateSubmitted = c.DateSubmitted
                    }).ToList()
            };

            // Pass the view model to the view
            return View(dashboardViewModel);
        }

        // POST: Approve a claim
        [HttpPost]
        public IActionResult ApproveClaim(int claimId)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
            {
                claim.Status = "Approved";
                _context.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }

        // POST: Reject a claim
        [HttpPost]
        public IActionResult RejectClaim(int claimId)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
            {
                claim.Status = "Rejected";
                _context.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }
    }
}
