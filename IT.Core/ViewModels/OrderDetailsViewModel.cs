using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class OrderDetailsViewModel
    {
        //customer attribute
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CustomerVehicleNumber { get; set; }
        public string CustomerDriverName { get; set; }
        public string CustomerOrderTime { get; set; }
        public int OrderQuantity { get; set; }

        //AWfule attribute        
        public string AcceptedBy { get; set; }
        public string DeliverdDate { get; set; }
        public string DeliverdVehicleNumber { get; set; }
        public string DeliverdDriverName { get; set; }
        public string Contact { get; set; }
        public string DeliverdOrderTime { get; set; }
        public int DeliverdOrderQuantity { get; set; }
        public string OrderProgress { get; set; }

    }
}
