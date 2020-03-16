using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class StockViewModel
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public string BillNo { get; set; }
        public string RefrenceNo { get; set; }
        public int VenderId { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal VAT { get; set; }
        public decimal Total { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UserName { get; set; }
        public string VenderName { get; set; }
        public string CreatedDates { get; set; }
        public int ItemId { get; set; }
        public int UsedQuantity { get; set; }       
    }
}
