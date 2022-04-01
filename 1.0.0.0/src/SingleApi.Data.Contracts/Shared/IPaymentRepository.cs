using SingleApi.Data.Contracts.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleApi.Data.Contracts.Shared
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Bank>> GetDependentBanksByBankIds(IEnumerable<string> ids);
        Task UpdateDependentBank(Bank bank);
        Task CreateDependentBank(Bank bank);
        Task UpdatePaymentChannelCurrencySetupByBankCode(string bankCode, decimal LimitMin, decimal LimitMax);
        Task UpdateBaseResourceStatusIdByBankCode(string bankCode, int statusId);
    }
}
