using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class DriverModel
    {
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public string TraficPlateNumber { get; set; }
        public string ContactNumber { get; set; }
        public int VehicleId { get; set; }
    }
}
