using Course.Core.Common;
using Course.Core.DTDs.Account;
using Course.Core.Services.Interfaces;
using Course.Core.Utility;
using Course.Core.Utility.Convertors;
using Course.Core.Utility.Generator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Course.Web.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _userService;
        private IViewRenderService _viewRenderService;

        public AccountController(IUserService userService, IViewRenderService viewRenderService)
        {
            _userService = userService;
            _viewRenderService = viewRenderService;
        }

        #region Register

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            if (_userService.IsExistUserName(registerViewModel.UserName))
            {
                ModelState.AddModelError("UserName", "نام کاربری معتبر نمی باشد");
            }

            if (_userService.IsExistEmail(FixedText.FixedEmail(registerViewModel.Email)))
            {
                ModelState.AddModelError("Email", "ایمیل معتبر نمی باشد");
            }

            if (!SafePassword.IsSafePasword(registerViewModel.Password))
            {
                ModelState.AddModelError("Password", "کلمه عبور ضعیف است!");
            }

            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            var user = _userService.RegisterUser(registerViewModel);

            #region Send Activation Email

            string body = _viewRenderService.RenderToStringAsync("_ActiveEmail", user);
            SendEmail.Send(user.Email, "فعالسازی", body);

            #endregion

            return View("RegisterSuccess", registerViewModel);
        }

        #endregion

        #region Login/LogOut

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var user = _userService.GetUser(loginViewModel);

            if (user != null)
            {
                if (user.IsActive)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim(ClaimTypes.Name,user.UserName)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = loginViewModel.RememberMe
                    };
                    HttpContext.SignInAsync(principal, properties);

                    ViewBag.IsSuccess = true;
                    return View();
                }
                else
                {
                    ModelState.AddModelError("Email", "حساب کابری فعال نمی باشد");
                }
            }
            else
            {
                ModelState.AddModelError("Email", "کاربری با مشخصات وارد شده بافت نشد!");
            }

            return View(loginViewModel);
        }


        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect(ConstValues.LoginPath);
        }

        #endregion

        #region Active Account

        public IActionResult ActiveAccount(string id)
        {
            ViewBag.IsSuccess = _userService.ActiveAccount(id);
            return View();
        }

        #endregion

        #region Forgot/Rest Password

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(forgotPasswordViewModel);
            }

            var user = _userService.GetUserByEmail(FixedText.FixedEmail(forgotPasswordViewModel.Email));

            if (user == null)
            {
                ModelState.AddModelError("Email", "کاربری یافت نشد!");
                return View(forgotPasswordViewModel);
            }

            #region Send Reset Pass Email

            string body = _viewRenderService.RenderToStringAsync("_ForgotPassword", user);
            SendEmail.Send(user.Email, "بازیابی کلمه عبور", body);

            #endregion

            ViewBag.IsSuccess = true;

            return View();
        }



        public IActionResult ResetPassword(string id)
        {

            return View(new ResetPasswordViewModel()
            {
                ActiveCode = id
            });
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {

            if (!ModelState.IsValid)
                return View(resetPasswordViewModel);

            var user = _userService.GetUserByActievCode(resetPasswordViewModel.ActiveCode);

            if (user == null)
            {
                return NotFound();
            }

            string hashNewPassword = PasswordHelper.EncodePasswordMd5(resetPasswordViewModel.Password);
            user.Password = hashNewPassword;
            user.ActiveCode = CodeGenerator.GenerateActiveCode();
            _userService.UpdateUser(user);

            ViewBag.IsSuccess = true;
            return View();

        }



        #endregion

    }
}
