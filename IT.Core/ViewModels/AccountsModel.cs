using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class AccountsModel
    {
        public int Id { get; set; }
        public string PONumber { get; set; }
        public decimal Total { get; set; }
        public decimal IssuedReceived { get; set; }
        public decimal Balance { get; set; }
        public string CreatedDates { get; set; }
        public string Name { get; set; }
        public string ReportHeading { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
