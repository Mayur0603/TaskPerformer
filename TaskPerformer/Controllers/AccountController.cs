using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using TaskPerformer.Models;
using TaskPerformer.Services;

namespace TaskPerformer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserServices _userServices;

        public AccountController(UserServices userServices)
        {
            _userServices = userServices;
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid)
            {
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                return View(model);
            }
            var success = await _userServices.RegisterUserAsync(model);
            if (success)
                return RedirectToAction("Login");

            ModelState.AddModelError("", "User Already Exists");
            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
           
            return RedirectToAction("Login", "Account");
        }



        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userServices.LoginUserAsync(model);
                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("FullName", $"{user.FirstName} {user.LastName}");
                    HttpContext.Session.SetString("FirstName", user.FirstName ?? "");
                    HttpContext.Session.SetString("LastName", user.LastName ?? "");
                    HttpContext.Session.SetString("Email", user.Email ?? "");
                    return RedirectToAction("Index", "Todo");
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }
            return View(model);
        }

        // GET: /Account/EditProfile
        public async Task<IActionResult> EditProfile()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            var user = await _userServices.GetUserByIdAsync(userId.Value);
            if (user == null) return NotFound();

            var model = new EditViewModel 
            {
                Username = user.Username,
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? "",
                Email = user.Email
            };

            return View(model);
        }

        // POST: /Account/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            
            var result = await _userServices.UpdateUserAsync(userId.Value, model);

            if (!result.Success)
            {
                ModelState.AddModelError("", "Failed to update profile. Username or Email may already be taken.");
                return View(model);
            }

            HttpContext.Session.SetString("Username", model.Username); // update session if username changed
            return RedirectToAction("Index", "Todo");
        }


    }
}
