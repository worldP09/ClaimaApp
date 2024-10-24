using Microsoft.AspNetCore.Mvc;
using ClaimApp.Models;
using ClaimApp.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims; // For claims
using Microsoft.AspNetCore.Authentication; // For authentication
using Microsoft.AspNetCore.Authentication.Cookies; // For cookie authentication
using System.Collections.Generic; // For List<T>

namespace ClaimApp.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ClaimAppContext _context;

        public LecturerController(ClaimAppContext context)
        {
            _context = context;
        }

        // GET: Lecturer/Index
        public IActionResult Index()
        {
            return View(); // This will look for Views/Lecturer/Index.cshtml
        }

        // GET: Lecturer/Login
        public IActionResult Login()
        {
            return View();
        }

       
        // POST: Lecturer/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) // Make this async
        {
            if (ModelState.IsValid)
            {
                var lecturer = _context.Lecturers
                    .FirstOrDefault(l => l.Email == model.Email && l.Password == model.Password);

                if (lecturer != null)
                {
                    // Create a list of security claims (System.Security.Claims.Claim)
                    var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, lecturer.LecturerId.ToString()), // Store Lecturer ID
                new System.Security.Claims.Claim(ClaimTypes.Name, lecturer.FirstName) // Store Lecturer Name
            };

                    // Create identity
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Set authentication properties (session persistence)
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true, // Keep the session alive
                    };

                    // Sign in the user with cookie authentication
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Dashboard"); // Redirect to dashboard after successful login
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid login credentials.";
                }
            }

            return View(model); // Return the login view with validation errors
        }

        // GET: Lecturer/Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SubmitClaim()
        {
            var model = new SubmitClaimViewModel
            {
                Modules = new List<ModuleViewModel> { new ModuleViewModel() } // Add at least one empty module
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SubmitApplication(SubmitClaimViewModel model)
        {
            if (ModelState.IsValid)
            {
                var lecturerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var lecturerName = $"{model.FirstName} {model.LastName}"; // Combine first and last names

                if (!int.TryParse(lecturerId, out int parsedLecturerId))
                {
                    ModelState.AddModelError("", "Invalid lecturer ID.");
                    return View("SubmitClaim", model);
                }

                foreach (var module in model.Modules)
                {
                    var claim = new ClaimApp.Models.Claim
                    {
                        LecturerId = parsedLecturerId,
                        LecturerName = lecturerName,
                        ModuleName = module.ModuleName,
                        HoursWorked = module.HoursWorked,
                        RatePerHour = module.RatePerHour,
                        TotalAmount = module.TotalAmount,
                        Status = "Pending",
                        DateSubmitted = DateTime.Now,
                        Description = model.Description
                    };

                    if (model.SupportingDocument != null && model.SupportingDocument.Length > 0)
                    {
                        var filePath = Path.Combine("uploads", model.SupportingDocument.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.SupportingDocument.CopyToAsync(stream);
                        }
                        // Save the file path to the claim if necessary
                        // claim.FilePath = filePath; // Uncomment if you have a FilePath property in Claim
                    }

                    _context.Claims.Add(claim);
                }

                try
                {
                    await _context.SaveChangesAsync();
                    ViewBag.StatusMessage = "Claim submitted successfully.";
                    return RedirectToAction("TrackClaims");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while submitting your claim. Please try again later.");
                    return View("SubmitClaim", model);
                }
            }

            return View("SubmitClaim", model);
        }
        // GET: Lecturer/TrackClaims
        public IActionResult TrackClaims()
        {
            var lecturerId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the Lecturer ID

            if (string.IsNullOrEmpty(lecturerId))
            {
                return RedirectToAction("Login", "Lecturer");
            }

            if (!int.TryParse(lecturerId, out int parsedLecturerId))
            {
                ModelState.AddModelError("", "Invalid lecturer ID.");
                return View(new TrackClaimsViewModel());
            }

            // Retrieve claims from the database and map them to TrackClaim
            var claims = _context.Claims.Where(c => c.LecturerId == parsedLecturerId).ToList();

            // Map claims to TrackClaim
            var trackClaimsList = claims.Select(c => new TrackClaim
            {
                Id = c.Id,
                ModuleName = c.ModuleName,
                HoursWorked = c.HoursWorked,
                TotalAmount = c.TotalAmount,
                DateSubmitted = c.DateSubmitted,
                Status = c.Status
            }).ToList();

            var viewModel = new TrackClaimsViewModel
            {
                ClaimsList = trackClaimsList
            };

            return View(viewModel); // Return the view with the view model
        }

        // GET: Lecturer/Profile
        [HttpGet]
        public IActionResult Profile()
        {
            // Retrieve the lecturer's ID from the User claims
            var lecturerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(lecturerId))
            {
                return RedirectToAction("Login", "Lecturer");
            }

            if (!int.TryParse(lecturerId, out int parsedLecturerId))
            {
                return RedirectToAction("Dashboard");
            }

            // Retrieve the lecturer's details from the database
            var lecturer = _context.Lecturers.FirstOrDefault(l => l.LecturerId == parsedLecturerId);

            if (lecturer == null)
            {
                return NotFound();
            }

            // Pass the lecturer details to the Profile view
            return View(lecturer);
        }
        // GET: Lecturer/EditProfile
        [HttpGet]
        public IActionResult EditProfile()
        {
            // Retrieve the lecturer's ID from the User claims
            var lecturerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(lecturerId))
            {
                return RedirectToAction("Login", "Lecturer");
            }

            if (!int.TryParse(lecturerId, out int parsedLecturerId))
            {
                return RedirectToAction("Dashboard");
            }

            // Retrieve the lecturer's details from the database
            var lecturer = _context.Lecturers.FirstOrDefault(l => l.LecturerId == parsedLecturerId);

            if (lecturer == null)
            {
                return NotFound();
            }

            // Pass the lecturer details to the view for editing
            return View("Profile", lecturer); // Use the same view for edit and display
        }
        // POST: Lecturer/EditProfile
        [HttpPost]
        public IActionResult EditProfile(Lecturer model)
        {
            if (!ModelState.IsValid)
            {
                // If the model state is not valid, redisplay the form
                return View("Profile", model);
            }

            // Retrieve the lecturer's ID from the User claims
            var lecturerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(lecturerId))
            {
                return RedirectToAction("Login", "Lecturer");
            }

            if (!int.TryParse(lecturerId, out int parsedLecturerId))
            {
                return RedirectToAction("Dashboard");
            }

            // Retrieve the lecturer's current details from the database
            var lecturer = _context.Lecturers.FirstOrDefault(l => l.LecturerId == parsedLecturerId);

            if (lecturer == null)
            {
                return NotFound();
            }

            // Update the lecturer's information with the posted values
            lecturer.FirstName = model.FirstName;
            lecturer.LastName = model.LastName;
            lecturer.Phone = model.Phone;
            lecturer.Department = model.Department;

            // Save the changes to the database
            _context.SaveChanges();

            // Optionally, redirect to the Profile page after saving changes
            return RedirectToAction("Profile");
        }

    }
}
