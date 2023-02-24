using LojaJkMisterG.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LojaJkMisterG.Controllers
{
    public class AccountAdmController : Controller
    {

        private readonly UserManager<IdentityUser> _admManager;
        private readonly SignInManager<IdentityUser> _signInadmManager;

        public AccountAdmController(UserManager<IdentityUser> admManager, SignInManager<IdentityUser> signInadmManager)
        {
            _admManager = admManager;
            _signInadmManager = signInadmManager;
        }

        //Área de admistração
        [AllowAnonymous]
        public IActionResult LoginAdm(string returnUrl)
        {
            return View(new LoginAdmViewModel()
            {
                ReturnUrl = returnUrl
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginAdm(LoginAdmViewModel loginAdm)
        {
            if (!ModelState.IsValid)
                return View(loginAdm);
            var user = await _admManager.FindByNameAsync(loginAdm.AdmName);

            if (user != null)
            {
                var result = await _signInadmManager.PasswordSignInAsync(user, loginAdm.PasswordAdm, false, false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginAdm.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Admin");
                    }

                    return Redirect(loginAdm.ReturnUrl);
                }
            }

            ModelState.AddModelError(string.Empty, "Credenciais incorretas!");
            return View("LoginAdm", loginAdm);

        }
    }
}
