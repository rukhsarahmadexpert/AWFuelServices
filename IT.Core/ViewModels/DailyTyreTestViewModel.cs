using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class DailyTyreTestViewModel
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime TyreTestDate { get; set; }
        public string TyreTestDates { get; set; }
        public string HeadFrontTypeRight { get; set; }
        public string HeadFrontTypeLeft { get; set; }
        public string HeadRearTyreRight_1_Pear { get; set; }
        public string HeadRearTyreRight_2_Pear { get; set; }
        public string HeadRearTyreLeft_1_Pear { get; set; }
        public string HeadRearTyreLeft_2_Pear { get; set; }
        public string TankerTyreRight_1 { get; set; }
        public string TankerTyreRight_2 { get; set; }
        public string TankerTyreRight_3 { get; set; }
        public string TankerTyreLeft_1 { get; set; }
        public string TankerTyreLeft_2 { get; set; }
        public string TankerTyreLeft_3 { get; set; }
        public string Remarks { get; set; }
        public string TankerNumber { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDates { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string TraficPlateNumber { get; set; }

    }
}

    
	
	