

using System;
using System.ComponentModel.DataAnnotations;

namespace Course.Core.DTDs
{
    public class WalletViewModel
    {
        public int WalletTypeId { get; set; }

        public int Amount { get; set; }

        public string Description { get; set; }

        public bool IsDone { get; set; }

        public DateTime CreateDate { get; set; }

        public string TrackingCode { get; set; }

    }

    public class WalletCharegeViewModel
    {
        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Amount { get; set; }

    }
}
