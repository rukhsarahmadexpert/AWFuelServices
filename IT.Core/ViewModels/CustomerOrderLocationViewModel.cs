using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class CustomerOrderLocationViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string LocationFullUrl { get; set; }
        public string PickingPoint { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public int DriverId { get; set; }
        public string UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public int SiteId { get; set; }
        public string FullName { get; set; }
        public string traficPlateNumber { get; set; }
    }
}

