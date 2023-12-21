using Course.Core.DTDs.AdminPanel;
using Course.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Course.Web.Pages.Admin.Users
{
    public class AddUserModel : PageModel
    {
        IUserService _userService;
        IRoleService _roleService;

        public AddUserModel(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        [BindProperty]
        public AddUserViewModel AddUserViewModel { get; set; }

        public void OnGet()
        {
            ViewData["Roles"] = _roleService.GetRoles();
        }

        public IActionResult OnPost(List<int> SelectedRoles)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = _roleService.GetRoles();
                return Page();
            }

            // TO DO

            // Validation:
            // User Exist
            // Email Exist
            // password weak
            // Avatar only Photo

            int userId = _userService.AddUserFromAdmin(AddUserViewModel, SelectedRoles);

            return Redirect("/Admin/Users/UserList");

        }
    }
}
