using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class DriverLoginHistoryViewModelForAdmin
    {
        public int DriverId { get; set; }
        public string Contact { get; set; }
        public string Name { get; set; }
        public int VehicleId { get; set; }
        public string TraficPlateNumber { get; set; }
        public bool IsActive { get; set; }
        public int DriverLoginId { get; set; }
        public string OrderStatus { get; set; }
        public string DriverImageUrl { get; set; }

    }
}
