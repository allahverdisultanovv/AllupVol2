using AllupVol2.Models;
using AllupVol2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllupVol2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM )
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }
            foreach (char c in registerVM.Name)
            {
                if (!Char.IsLetter(c))
                {
                    ModelState.AddModelError(nameof(registerVM.Name), "Name can be exists only letters");
                    return View();
                }

            }
            foreach (char c in registerVM.Surname)
            {
                if (!Char.IsLetter(c))
                {
                    ModelState.AddModelError(nameof(registerVM.Surname), "Surname can be exists only letters");
                    return View();
                }
            }
            AppUser user = new AppUser()
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
            };

            IdentityResult result =await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded) 
            {
                foreach (var error in result.Errors)
                {
                    if (error.Description.Contains("Password"))
                    {
                        ModelState.AddModelError(nameof(registerVM.Password), error.Description);
                        return View();
                    }
                    if (error.Description.Contains("Email"))
                    {
                        ModelState.AddModelError(nameof(registerVM.Email), error.Description);
                        return View();
                    }
                    if (error.Description.Contains("UserName"))
                    {
                        ModelState.AddModelError(nameof(registerVM.Username), error.Description);
                        return View();
                    }
                    return View();
                }
            }
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index","Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM,string? returnurl)
        {
            if(!ModelState.IsValid)return View();
            
            if(returnurl is null)
            {
                RedirectToAction("Index", "Home");
            }
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginVM.UserOrEmail || u.Email == loginVM.UserOrEmail);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Your account hass been locked . Please try again later");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsPersistant, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Your account hass been locked . Please try again later");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email UserName or Password is incorrect");
                return View();
            }
            if(returnurl is null)return RedirectToAction("Index", "Home");
            return Redirect(returnurl);
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
