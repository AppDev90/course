
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Course.DataLayer.Entities.User
{
    public class Role
    {
        public int RoleId { get; set; }

        [Display(Name = "نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(64, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string RoleTitle { get; set; }

        #region Relations

        public List<UserRole> UserRoles { get; set; }

        #endregion

    }
}
