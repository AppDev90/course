using Course.Core.Common;
using Course.Core.DTDs;
using Course.Core.Services.Interfaces;
using Course.DataLayer.Context;
using Course.DataLayer.Entities.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Core.Services
{
    public class WalletService : IWalletService
    {
        private readonly CourseDbContext _context;

        public WalletService(CourseDbContext courseDbContext)
        {
            this._context = courseDbContext;
        }

        public int GetWalletAmount(int userId)
        {
            var signedAmount = _context.Wallets.Where(p => p.UserId == userId && p.IsDone == true)
                 .Select(p => new
                 {
                     amount = p.WalletTypeId == 1 ? p.Amount : -1 * p.Amount
                 });
            return signedAmount.Any() ? signedAmount.Sum(p => p.amount) : 0;
        }

        public List<WalletViewModel> GetWalletByUserId(int userId)
        {
            return _context.Wallets
                .Where(p => p.UserId == userId)
                .Select(p => new WalletViewModel
                {
                    Amount = p.Amount,
                    CreateDate = p.CreateDate,
                    Description = p.Description,
                    IsDone = p.IsDone,
                    TrackingCode = p.TrackingCode,
                    WalletTypeId = p.WalletTypeId,
                }).ToList();
        }

        public int ChargeWallet(int userId, int amount, string description, bool isDone = false)
        {
            var wallet = new Wallet()
            {
                Amount = amount,
                Description = description,
                IsDone = isDone,
                UserId = userId,
                WalletTypeId = 1,
                CreateDate = DefaultValues.DateTimeNow()
            };

            return AddWallet(wallet);
        }

        public int AddWallet(Wallet wallet)
        {

            _context.Wallets.Add(wallet);

            _context.SaveChanges();

            return wallet.WalletId;
        }

        public Wallet GetWallet(int id)
        {
            return _context.Wallets.SingleOrDefault(p => p.WalletId == id);
        }

        public void UpdateWallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            _context.SaveChanges();
        }
    }
}
