using SingleApi.Svc.Contracts.Paysafe.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleApi.Svc.Contracts.Paysafe.Models.View
{
    public class CreatePaymentHandleViewModel
    {
        public string RedirectUrl { get; set; }
    }
}
