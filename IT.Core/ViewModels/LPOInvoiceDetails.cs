using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class LPOInvoiceDetails
    {
        public int Id { get; set; }
        public int LPOId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public int UnitId { get; set; }

        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int Qunatity { get; set; }
        public decimal Total { get; set; }

        public decimal VAT { get; set; }
        public decimal SubTotal { get; set; }
    }
}
