using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class AccountStatisticsViewModel
    {
        public decimal Receivable { get; set; }
        public decimal Received { get; set; }
        public decimal Balance { get; set; }
    }
}
