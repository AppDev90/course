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
    public class EditUserModel : PageModel
    {
        private readonly IUserService userServicec;
        private readonly IRoleService roleService;

        public EditUserModel(IUserService userServicec, IRoleService roleService)
        {
            this.userServicec = userServicec;
            this.roleService = roleService;
        }

        [BindProperty]
        public EditUserViewModel EditUserViewModel { get; set; }

        public void OnGet(int id)
        {
            EditUserViewModel = userServicec.GetUserForEdit(id);
            ViewData["Roles"] = roleService.GetRoles();
        }

        public IActionResult OnPost(List<int> SelectedRoles)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // TO DO

            // Validation:
            // User Exist
            // Email Exist
            // password weak
            // Avatar only Photo

            userServicec.EditUser(EditUserViewModel, SelectedRoles);

            return RedirectToPage("UserList");
        }
    }
}
