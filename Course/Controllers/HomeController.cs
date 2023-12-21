using Course.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PaymentCallBack(int id, [FromServices] IWalletService walletService)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
               HttpContext.Request.Query["Status"].ToString().ToLower() == "ok"
               && HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"];
                
                var wallet = walletService.GetWallet(id);
                var payment = new ZarinpalSandbox.Payment(wallet.Amount);

                var res = payment.Verification(authority).Result;
                
                if (res.Status == 100)
                {
                    ViewBag.code = res.RefId;
                    ViewBag.IsSuccess = true;

                    wallet.TrackingCode = res.RefId.ToString();
                    wallet.IsDone = true;

                    walletService.UpdateWallet(wallet);

                }

            }

            return View();
        }
    }
}
