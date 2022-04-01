using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleApi.Data.Contracts.Models.BatchProcess
{
    public enum SyExecProcessStatus : short
    {
        Ready = 1,
        Running = 2,
        CompletedOK = 3,
        CompletedWithErrors = 4,
        Failed = 5,
        Cancelled = 6
    }
}
