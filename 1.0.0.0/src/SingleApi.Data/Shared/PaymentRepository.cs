using Dapper;
using SingleApi.Data.Base;
using SingleApi.Data.Contracts.Models.Payment;
using SingleApi.Data.Contracts.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleApi.Data.Shared
{
    public class PaymentRepository : BaseRepository, IPaymentRepository
    {
        public PaymentRepository(string connectionString) : base(connectionString)
        {

        }

        public async Task CreateDependentBank(Bank bank)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ParentBankCode", bank.ParentBankCode);
            parameters.Add("BankCode", bank.BankCode);
            parameters.Add("BankName", bank.BankName);
            parameters.Add("BankUrl", bank.BankUrl);
            parameters.Add("BankLabel", bank.BankLable);
            parameters.Add("Enabled", bank.Enabled);
            parameters.Add("CreatedBy", bank.CreatedBy);
            parameters.Add("DateCreated", DateTime.UtcNow);
            parameters.Add("SortOrder", bank.SortOrder);
            parameters.Add("LimitMin", bank.LimitMin);
            parameters.Add("LimitMax", bank.LimitMax);
            parameters.Add("CurrencyId", bank.CurrencyId);
            parameters.Add("IsShownInExpress", bank.IsShownInExpress);
            parameters.Add("LegalType", bank.LegalType);

            await ExecuteAsync("[dbo].[uspInsertDependentBank]", parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Bank>> GetDependentBanksByBankIds(IEnumerable<string> ids)
        {
            var idsString = string.Join(",", ids);
            var parameters = new DynamicParameters();
            parameters.Add("BankIds", idsString);
            return await GetFilteredListAsync<Bank>("[dbo].[uspGetDependentBankByIds]", parameters, CommandType.StoredProcedure);
        }

        public async Task UpdateBaseResourceStatusIdByBankCode(string bankCode, int statusId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("BankCode", bankCode);
            parameters.Add("StatusId", statusId);

            await ExecuteAsync("[dbo].[uspUpdateBaseResourceStatusIdByBankCode]", parameters, CommandType.StoredProcedure);
        }

        public async Task UpdatePaymentChannelCurrencySetupByBankCode(string bankCode, decimal LimitMin, decimal LimitMax)
        {
            var parameters = new DynamicParameters();
            parameters.Add("BankCode", bankCode);
            parameters.Add("LimitMin", LimitMin);
            parameters.Add("LimitMax", LimitMax);

            await ExecuteAsync("[dbo].[uspUpdatePaymentChannelCurrencySetupByBankCode]", parameters, CommandType.StoredProcedure);
        }

        public async Task UpdateDependentBank(Bank bank)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", bank.DependentBankId);
            parameters.Add("ParentBankCode", bank.ParentBankCode);
            parameters.Add("BankCode", bank.BankCode);
            parameters.Add("BankName", bank.BankName);
            parameters.Add("BankUrl", bank.BankUrl);
            parameters.Add("BankLabel", bank.BankLable);
            parameters.Add("Enabled", bank.Enabled);
            parameters.Add("ModifiedBy", bank.ModifiedBy);
            parameters.Add("DateModified", DateTime.UtcNow);
            parameters.Add("SortOrder", bank.SortOrder);
            parameters.Add("LimitMin", bank.LimitMin);
            parameters.Add("LimitMax", bank.LimitMax);
            parameters.Add("CurrencyId", bank.CurrencyId);
            parameters.Add("IsShownInExpress", bank.IsShownInExpress);
            parameters.Add("LegalType", bank.LegalType);

            await ExecuteAsync("[dbo].[uspUpdateDependentBank]", parameters, CommandType.StoredProcedure);
        }
    }
}
