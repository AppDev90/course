using Course.Core.Common;
using Course.Core.DTDs;
using Course.Core.Services.Interfaces;
using Course.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Course.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            this._userService = userService;
        }

        public IActionResult Index()
        {
            var userInfo = _userService.GetuserInformation(User.GetUserId());

            return View(userInfo);
        }


        public IActionResult EditProfile()
        {
            return View(_userService.GetUserPofile(User.GetUsername()));
        }

        [HttpPost]
        public IActionResult EditProfile(EditProfileViewModel editProfileViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editProfileViewModel);
            }

            if (_userService.IsExistUsenameExistEdit(User.GetUserId(), User.GetUsername()))
            {
                ModelState.AddModelError("UserName", "نام کاربری تکراری است.");
                return View(editProfileViewModel);
            }

            if (editProfileViewModel.UserAvatar != null)
            {
                if (!editProfileViewModel.UserAvatar.IsImage())
                {
                    ModelState.AddModelError("UserAvatar", "لطفاً فقط تصویر انتخاب نمایید.");
                    return View(editProfileViewModel);
                }
            }

            _userService.EditUserProfile(User.GetUsername(), editProfileViewModel);

            ViewBag.IsSuccess = true;

            ViewBag.UsernameChanged = User.GetUsername() != editProfileViewModel.UserName;

            return View(editProfileViewModel);
        }


        public IActionResult EditPassword()
        {

            return View();
        }

        [HttpPost]
        public IActionResult EditPassword(EditPasswordViewModel editPasswordViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(editPasswordViewModel);
            }

            if (!editPasswordViewModel.Password.IsSafePasword())
            {
                ModelState.AddModelError("Password", "کلمه عبور جدید ساده است");
                return View(editPasswordViewModel);
            }

            string userName = User.GetUsername();

            if (!_userService.IsePasswordCorrect(userName, PasswordHelper.EncodePasswordMd5(editPasswordViewModel.Password)))
            {
                ModelState.AddModelError("CurrentPassword", "کلمه عبور فعلی صحیح نمی باشد");
                return View(editPasswordViewModel);
            }

            _userService.ChangePassword(userName,PasswordHelper.EncodePasswordMd5(editPasswordViewModel.Password));
            
            ViewBag.IsSuccess = true;

            return View();
        }
    }


}
