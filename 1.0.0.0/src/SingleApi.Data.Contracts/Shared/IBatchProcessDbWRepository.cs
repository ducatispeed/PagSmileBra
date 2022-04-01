using SingleApi.Data.Contracts.Models.BatchProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleApi.Data.Contracts.Shared
{
    public interface IBatchProcessDbWRepository
    {
        Task<IDictionary<string, string>> GetSyAppSvcConfigSettingsAsync(string syAppSvcCode);
        Task<IEnumerable<KeyValuePair<string, string>>> GetSyAppSvcConfigSettings(string syAppSvcCode, string parentSettingKey);
        Task<string> GetSyAppSvcConfigSettingAsync(string syAppSvcCode, string settingKey);
        Task<Guid> GetSyExecProcessGId(string syAppSvcCode);
        Task Insert(SyExecProcessInstance model);
        Task Update(SyExecProcessInstance model);
        Task<SyExecProcessInstance> GetLastProcessInstanceAsync(string processName);
        Task<SyExecProcessInstance> GetLastProcessInstanceAsync(
            string processName,
            SyExecProcessStatus status);
        Task<IEnumerable<CronFiredTrigger>> GetCronFiredTriggersAsync(string jobName);
    }
}
