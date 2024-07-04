using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pronia.Extencions.Enums;
using Pronia.Models;
using Pronia.Models.ViewModels;

namespace Pronia.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser>signInManager,RoleManager<IdentityRole>roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public async Task< IActionResult> Test()
        {
            foreach(UserRoleEnum role in Enum.GetValues(typeof(UserRoleEnum)))
            {
             if(!await roleManager.RoleExistsAsync(role.ToString()))  await roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });

            }
            return Ok();

        }
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return NotFound();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            AppUser newUser = new AppUser
            {
                LastName = vm.LastName,
                Email = vm.Email,
                FirstName = vm.FirstName,
                UserName = vm.UserName,

            };
            IdentityResult res = await userManager.CreateAsync(newUser,vm.Password);
            if (!res.Succeeded)
            {
                foreach (var err in res.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }
                    return View(vm);
            }
            await userManager.AddToRoleAsync(newUser, UserRoleEnum.Member.ToString());
            await signInManager.SignInAsync(newUser, false);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return NotFound();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm,string returnUrl)
        {
            if (!ModelState.IsValid) return View(vm);
            AppUser user = await userManager.FindByNameAsync(vm.UserOrEmail);
            if (user == null)
            {
                user =await userManager.FindByEmailAsync(vm.UserOrEmail);
                if(user==null)
                {
                    
                    ModelState.AddModelError(string.Empty, "UserName or Password incorrent!");
                    return View(vm);
                }
            }
           var res= await signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, true);
            if(!res.Succeeded)
            {
                if (res.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "You have been blocked  for 10 second!!!");
                    return View(vm);
                }
                ModelState.AddModelError(string.Empty, "UserName or Password incorrent!");
                return View(vm);
            }

            return Redirect(returnUrl);
        }
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
