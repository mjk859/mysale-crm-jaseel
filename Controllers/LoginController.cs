using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MySaleApp.Admin.UI.Context;
using MySaleApp.Admin.UI.Models;
using MySaleApp.Admin.UI.ViewModel;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;

namespace MySaleApp.Admin.UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly MySaleAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;
        private readonly IDistributedCache _cache;

        public LoginController(MySaleAppContext context, IConfiguration configuration, ILogger<LoginController> logger, IDistributedCache cache)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            _logger.LogInformation("Login attempt started for user: {Username}", model.UserName);

            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Model state invalid for user: {Username}", model.UserName);
                    return View(model);
                }

                // Test database connection first
                _logger.LogInformation("Testing database connection...");
                var userCount = await _context.SystemUsers.CountAsync();
                _logger.LogInformation("Database connection successful. Found {UserCount} users in table.", userCount);


                // Find user
                var user = await _context.SystemUsers
                    .FirstOrDefaultAsync(u => u.UserName == model.UserName);

                if (user != null)
                {
                    _logger.LogInformation("User {Username} found in database", model.UserName);

                    var passwordHasher = new PasswordHasher<SystemUser>();
                    var result = passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

                    if (result == PasswordVerificationResult.Success)
                    {
                        _logger.LogInformation("Password verification successful for user: {Username}", model.UserName);


                        
                        // Create authentication cookie
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Email, user.Email ?? "")
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));

                        _logger.LogInformation("User {Username} signed in successfully, redirecting to Dashboard", model.UserName);

                        HttpContext.Response.Redirect("/Dashboard");
                        return new EmptyResult();

                        //// a small delay to ensure cookie is set
                        //await Task.Delay(100);

                        //return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        _logger.LogWarning("Invalid password for user: {Username}", model.UserName);
                        ModelState.AddModelError("Password", "Invalid password");
                    }

                    
                }
                else
                {
                    _logger.LogWarning("User {Username} not found in database", model.UserName);
                    ModelState.AddModelError("UserName", "User not found");
                }

                ModelState.AddModelError("", "Invalid username or password");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user: {Username}", model.UserName);
                ModelState.AddModelError("", "An error occurred during login. Please try again.");
            }

            _logger.LogInformation("Login attempt failed for user: {Username}, returning to login page", model.UserName);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var username = HttpContext.User.Identity?.Name;
            _logger.LogInformation("User {Username} logging out", username);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            _logger.LogInformation("User {Username} logged out successfully", username);
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Get logged-in user info
            var username = HttpContext.User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Index", "Login");
            }

            var user = await _context.SystemUsers.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View(model);
            }

            var hasher = new PasswordHasher<SystemUser>();
            var passwordCheck = hasher.VerifyHashedPassword(user, user.Password, model.CurrentPassword);

            if (passwordCheck != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
                return View(model);
            }

            // Update to new password
            user.Password = hasher.HashPassword(user, model.NewPassword);
            await _context.SaveChangesAsync();

            ViewBag.Message = "Password changed successfully!";
            return RedirectToAction("Index", "Dashboard");
        }


    }
}