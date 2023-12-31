﻿
using System.ComponentModel.DataAnnotations;


namespace Course.Core.DTDs.Account
{
    public class ForgotPasswordViewModel
    {

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(32, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }

    }
}
