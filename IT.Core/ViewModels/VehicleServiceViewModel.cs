using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class VehicleServiceViewModel
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime ServiceDate { get; set; }
        public string ServiceDates { get; set; }
        public string Engine { get; set; }
        public string Mileage { get; set; }
        public string OilFilter { get; set; }
        public string DieselFilter { get; set; }
        public string GearOil { get; set; }
        public string BodyGreas { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string TraficPlateNumber { get; set; }
        public int TotalRows { get; set; }
    }
}

