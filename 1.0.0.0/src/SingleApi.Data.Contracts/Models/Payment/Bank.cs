using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleApi.Data.Contracts.Models.Payment
{
    public class Bank
    {
        public int DependentBankId { get; set; }
        public string ParentBankCode { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BankUrl { get; set; } = "";
        public string BankLable { get; set; } = "";
        public bool Enabled { get; set; }
        public string ModifiedBy { get; set; } = "0";
        public string CreatedBy { get; set; } = "0";
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateModified { get; set; } = DateTime.UtcNow;
        public short? SortOrder { get; set; }
        public decimal LimitMin { get; set; }
        public decimal LimitMax { get; set; }
        public string CurrencyId { get; set; }
        public bool IsShownInExpress { get; set; }
        public short LegalType { get; set; }

    }
}
