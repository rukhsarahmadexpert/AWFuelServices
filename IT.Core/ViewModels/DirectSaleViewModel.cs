using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class DirectSaleViewModel
    {
        public int VehicleId { get; set; }
        public string TraficPlateNumber { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public int DriverId { get; set; }
        public string ContactNumber { get; set; }
        public string Name { get; set; }
        public string DriverName { get; set; }
    }
}
