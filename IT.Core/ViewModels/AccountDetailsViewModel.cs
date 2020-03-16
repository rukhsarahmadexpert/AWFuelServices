using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class AccountDetailsViewModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int BillId { get; set; }
        public int InvoiceId { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal PayedAmount { get; set; }
        public decimal Balance { get; set; }
        public decimal GrandTotal { get; set; }

    }
}
