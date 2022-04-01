using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleApi.Infrastructure.Enums
{
    public enum BankStatus
    {
        Pending = 0,
        Expired = 1,
        Paid = 2,
        AlreadyPaid = 3,
        NotFound = 4,
        Void = 5
    }
}
