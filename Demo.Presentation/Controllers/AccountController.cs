global using Demo.DataAccess.Models;
using Demo.BusinessLogic.Services.EmailSettings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

namespace Demo.Presentation.Controllers
{
    public class AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IEmailSettings emailSettings) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IEmailSettings _emailSettings = emailSettings;

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = _userManager.CreateAsync(user, model.Password).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
        #endregion

        #region Login


        [HttpGet]
        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var user = _userManager.FindByEmailAsync(model.Email).Result;
            if (user is not null)
            {
                if (_userManager.CheckPasswordAsync(user, model.Password).Result)
                {
                    var result = _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false).Result;

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid Email Or Password");

            return View();
        }

        #endregion

        public IActionResult SignOut()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(forgetPasswordViewModel.Email);
                if(user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var url = Url.Action("ResetPassword","Account",new {email = forgetPasswordViewModel.Email , token = token},Request.Scheme);
                    var email = new Email()
                    {
                        To = forgetPasswordViewModel.Email,
                        Subject = "Reset Your Password",
                        Body = url,
                    };
                    _emailSettings.SendEmail(email);
                    return RedirectToAction("CheckYourInbox");

                }
                ModelState.AddModelError(string.Empty, "Invalid Operation, Please Try Again");

            }
            return View(forgetPasswordViewModel);

        }
        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;
                var user = await _userManager.FindByEmailAsync(email);
                if(user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token,resetPasswordViewModel.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Login));
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid operation try again");
            return View(resetPasswordViewModel);
        }
    }
}
