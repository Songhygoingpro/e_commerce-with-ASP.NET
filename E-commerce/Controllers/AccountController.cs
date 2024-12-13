using E_commerce.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountDataAccess _dataAccess;

        public AccountController(AccountDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        // Login (GET)
        public IActionResult Login()
        {
            return View();
        }

        // Login (POST)
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            // Authenticate the user
            User loggedInUser = _dataAccess.Authenticate(user.Username, user.Password);

            if (loggedInUser != null)
            {
                // Create claims for the user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loggedInUser.Username),
                    new Claim(ClaimTypes.Role, loggedInUser.IsAdmin ? "Admin" : "User"),
                    new Claim("IsAdmin", loggedInUser.IsAdmin.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Sign in the user using cookie authentication
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                // Redirect user based on role
                if (loggedInUser.IsAdmin)
                    return RedirectToAction("Index", "Product"); // Admin Dashboard
                else
                    return RedirectToAction("Index", "Home"); // Home for regular user
            }

            // If authentication fails, show error message
            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }

        // Logout
        public async Task<IActionResult> Logout()
        {
            // Sign out and clear the cookies
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // Sign Up (GET)
        public IActionResult SignUp()
        {
            return View();
        }

        // Sign Up (POST)
        [HttpPost]
        public IActionResult SignUp(User user)
        {
            if (ModelState.IsValid)
            {
                // Register the user (add them to the database or data store)
                _dataAccess.RegisterUser(user);
                return RedirectToAction("Login");
            }
            return View(user);
        }
    }
}
