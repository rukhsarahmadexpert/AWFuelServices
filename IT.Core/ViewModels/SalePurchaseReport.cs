using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class SalePurchaseReport
    {
        public int Id { get; set; }
        public string PONumber { get; set; }
        public string LPONumber { get; set; }
        public decimal Total { get; set; }
        public decimal VAT { get; set; }
        public decimal GrandTotal { get; set; }
        public string FromDates { get; set; }
        public string DueDates { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string ReportHeading { get; set; }
        public int Quantity { get; set; }
        public int WithDraw { get; set; }
        public decimal Balance { get; set; }
        public string CreatedDates { get; set; }
        public decimal IssuedMoney { get; set; }
    }
}
