using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.Core.DTDs.AdminPanel;
using Course.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Course.Web.Pages.Admin.Users
{
    public class IndexModel : PageModel
    {

        IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public UserListViweModel UserListViweModel { get; set; }

        public void OnGet(int pageId = 1, string filterUserName = "", string filterEmail = "", bool filterIsDeleted = false)
        {
            UserListViweModel = _userService.GetUsers(pageId, filterEmail, filterUserName, filterIsDeleted);

            UserListViweModel.FilterEmail = filterEmail;
            UserListViweModel.FilterUserName = filterUserName;
            UserListViweModel.FilterIsDeleted = filterIsDeleted;

        }

        public IActionResult OnGetDelete(int id)
        {
            _userService.DeleteUser(id);
            return RedirectToPage("UserList");
        }

    }
}
