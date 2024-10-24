using ClaimApp.Models;
using ClaimApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

public class CoordinatorController : Controller
{
    private readonly ClaimAppContext _context;

    public CoordinatorController(ClaimAppContext context)
    {
        _context = context;
    }

    // GET: Coordinator/Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: Coordinator/Login
    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var coordinator = _context.Coordinators
                .FirstOrDefault(c => c.Email == model.Email && c.Password == model.Password);

            if (coordinator != null)
            {
                // Store coordinator ID or name in session
                HttpContext.Session.SetInt32("CoordinatorId", coordinator.CoordinatorId); // Save Coordinator Id in session
                HttpContext.Session.SetString("CoordinatorName", coordinator.FirstName); // Save Coordinator Name in session

                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid login credentials.";
            }
        }
        return View(model);
    }

    // GET: Coordinator/Dashboard
    public IActionResult Dashboard()
    {
        // Get the coordinator ID and name from session
        var coordinatorId = HttpContext.Session.GetInt32("CoordinatorId");
        var coordinatorName = HttpContext.Session.GetString("CoordinatorName");

        if (coordinatorId == null)
        {
            return RedirectToAction("Login"); // Redirect if session expired or not logged in
        }

        // Retrieve pending claims for the coordinator dashboard
        var viewModel = new CoordinatorDashboardViewModel
        {
            CoordinatorName = coordinatorName, // Use session-stored coordinator name
            TotalClaims = _context.Claims.Count(),
            ApprovedClaims = _context.Claims.Count(c => c.Status == "Approved"),
            PendingClaims = _context.Claims.Count(c => c.Status == "Pending"),
            RejectedClaims = _context.Claims.Count(c => c.Status == "Rejected"),
            PendingClaimsList = _context.Claims.Where(c => c.Status == "Pending").ToList()
        };

        return View(viewModel);
    }

    // POST: Approve Claim
    [HttpPost]
    public IActionResult ApproveClaim(int claimId)
    {
        var claim = _context.Claims.FirstOrDefault(c => c.Id == claimId);
        if (claim != null)
        {
            claim.Status = "Approved";
            _context.SaveChanges();
        }
        return RedirectToAction("TrackClaims");
    }
    [HttpPost]
    public IActionResult RejectClaim(int claimId, string rejectionReason)
    {
        var claim = _context.Claims.FirstOrDefault(c => c.Id == claimId);

        if (claim != null)
        {
            claim.Status = "Rejected";
            claim.Description = rejectionReason; // Optionally store the rejection reason
            _context.SaveChanges(); // Save the updated status in the database
        }

        return RedirectToAction("TrackClaims");
    }

    // POST: Reject Claim
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
    
    // GET: Lecturer/EditProfile
    [HttpGet]
    public IActionResult EditProfile()
    {
        // Retrieve the lecturer's ID from the User claims
        var lecturerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(lecturerId))
        {
            // If the lecturer is not logged in, redirect to the login page
            return RedirectToAction("Login", "Lecturer");
        }

        // Convert lecturerId to an integer
        if (!int.TryParse(lecturerId, out int parsedLecturerId))
        {
            ModelState.AddModelError("", "Invalid lecturer ID.");
            return RedirectToAction("Dashboard");
        }

        // Retrieve the lecturer's details from the database
        var lecturer = _context.Lecturers.FirstOrDefault(l => l.LecturerId == parsedLecturerId);

        if (lecturer == null)
        {
            return NotFound();
        }

        // Pass the lecturer details to the view for editing
        return View(lecturer);
    }

    // POST: Lecturer/EditProfile
    [HttpPost]
    public IActionResult EditProfile(Lecturer model)
    {
        if (!ModelState.IsValid)
        {
            return View(model); // Return the view with the model if validation fails
        }

        // Retrieve the lecturer's ID from the User claims
        var lecturerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(lecturerId))
        {
            return RedirectToAction("Login", "Lecturer");
        }

        if (!int.TryParse(lecturerId, out int parsedLecturerId))
        {
            ModelState.AddModelError("", "Invalid lecturer ID.");
            return View(model);
        }

        // Find the lecturer in the database
        var lecturer = _context.Lecturers.FirstOrDefault(l => l.LecturerId == parsedLecturerId);

        if (lecturer == null)
        {
            return NotFound();
        }

        // Update the lecturer's details
        lecturer.FirstName = model.FirstName;
        lecturer.LastName = model.LastName;
        lecturer.Email = model.Email;
        lecturer.Phone = model.Phone;
        lecturer.Department = model.Department;

        // Save the changes to the database
        _context.SaveChanges();

        ViewBag.SuccessMessage = "Profile updated successfully.";

        return View(lecturer); // Return the updated model back to the view
    }


}
