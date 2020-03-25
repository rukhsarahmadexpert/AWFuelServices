using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class BookLeftRemainingViewModel
    {
        public int CompanyId { get; set; }
        public int TotalBooked { get; set; }
        public int TotalLefted { get; set; }
        public int QuantityRemaining { get; set; }
        public string ProductName { get; set; }
    }
}
