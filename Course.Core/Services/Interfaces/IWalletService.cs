
using Course.Core.DTDs;
using Course.DataLayer.Entities.Wallet;
using System.Collections.Generic;

namespace Course.Core.Services.Interfaces
{
    public interface IWalletService
    {
        int GetWalletAmount(int userId);

        List<WalletViewModel> GetWalletByUserId(int userId);

        int ChargeWallet(int userId, int amount, string Description, bool isDone = false);

        int AddWallet(Wallet wallet);

        Wallet GetWallet(int id);

        void UpdateWallet(Wallet wallet);

    }
}
