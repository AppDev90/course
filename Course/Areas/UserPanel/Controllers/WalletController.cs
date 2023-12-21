using Course.Core.Common;
using Course.Core.DTDs;
using Course.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]

    public class WalletController : Controller
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            this._walletService = walletService;
        }

        public IActionResult Index()
        {
            ViewBag.Wallet = _walletService.GetWalletByUserId(User.GetUserId());

            return View();
        }

        [HttpPost]
        public IActionResult Index(WalletCharegeViewModel walletCharegeViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Wallet = _walletService.GetWalletByUserId(User.GetUserId());
                return View(walletCharegeViewModel);
            }

            int walletId = _walletService.ChargeWallet(User.GetUserId(), walletCharegeViewModel.Amount, "شارژ حساب", false);

            #region Online Payment

            var payment = new ZarinpalSandbox.Payment(walletCharegeViewModel.Amount);

            var res = payment.PaymentRequest("شارژ حساب", ConstValues.DomainPath + "Home/PaymentCallBack/" + walletId);

            if (res.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
            }

            #endregion

            return null;
        }

    }
}
