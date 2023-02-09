using LojaJkMisterG.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace LojaJkMisterG.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnurl)
        {
            return View(new LoginViewModel()
            {
                ReturnUrl = returnurl,
            });
        }

        [HttpPost]

        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _userManager.FindByNameAsync(loginVM.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginVM.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    return Redirect(loginVM.ReturnUrl);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CadastroViewModel registroVM)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = registroVM.UserName };

                var email = await _userManager.CreateAsync(user, registroVM.EmailRegister);

                var result = await _userManager.CreateAsync(user,registroVM.Password);

                var result2 = await _userManager.CreateAsync(user, registroVM.PasswordConfirm);

                var existingUser = await _userManager.FindByNameAsync(registroVM.UserName);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "O nome de usuário já existe");
                    return View(registroVM);
                }

                if (result.Succeeded && email.Succeeded && result2.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    this.ModelState.AddModelError( "Registro", "Falha ao registrar o usuário");
                }

            }

            return View(registroVM);
        }
    }
}
