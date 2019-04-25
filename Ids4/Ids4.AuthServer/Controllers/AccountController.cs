using IdentityServer4.Test;
using Ids4.AuthServer.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;

namespace Ids4.AuthServer.Controllers
{
    public class AccountController: Controller
    {
        private readonly TestUserStore _users;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public AccountController(TestUserStore users,IIdentityServerInteractionService identityServerInteractionService)
        {
            _identityServerInteractionService = identityServerInteractionService;
            _users = users;
        }

        public ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {//如果是本地
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(HomeController.Index),"Home");
        }

        private void AddError(IdentityResult result)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError(string.Empty, item.Description);
            }
        }

        public IActionResult Register(string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        //public async Task<IActionResult> Register()

        public IActionResult Login(string returnUrl =null)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }
            
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel,string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                ViewData["returnUrl"] = returnUrl;
                var user = _users.FindByUsername(loginViewModel.UserNmae);

                if (user == null)
                {
                    ModelState.AddModelError(nameof(loginViewModel.UserNmae), "UserName is exists");
                }
                else
                {
                    if (_users.ValidateCredentials(loginViewModel.UserNmae,loginViewModel.Password))
                    {
                        //是否记住
                        var prop = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(30))
                        };

                        await Microsoft.AspNetCore.Http.AuthenticationManagerExtensions.SignInAsync(HttpContext, user.SubjectId, user.Username, prop);
                    }

                    return RedirectToLocal(returnUrl);
                }
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("index","Home");
        }
        
    }
}
