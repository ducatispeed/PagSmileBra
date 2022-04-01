using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleApi.Infrastructure.Constants
{
    public class PaymentRequest
    {
        public const string TransactionType = "PAYMENT";
        public const string PaymentType = "RAPID_TRANSFER";
        public const string CallbackMethod = "GET";
        public const string OnCompletedCallback = "on_completed";
        public const string OnFailedCallback = "on_failed";
    }
}
