using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.DataLayer.Entities.Wallet
{
    public class WalletType
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WalletTypeId { get; set; }

        [Required]
        [MaxLength(32)]
        public string TypeTittle { get; set; }

        public List<Wallet> Wallets { get; set; }


    }
}
