using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class OrderReport
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string VehicleNumber { get; set; }
        public int OrderQuantity { get; set; }
        public int DeliverQuantity { get; set; }
        public string Status { get; set; }
    }
}
