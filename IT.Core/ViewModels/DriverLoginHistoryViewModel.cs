using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class DriverLoginHistoryViewModel
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int DriverId { get; set; }
        public string Date { get; set; }
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }
        public string LoginTime { get; set; }
        public string LogoutTime { get; set; }
        public string userName { get; set; }
        public string DriverImageUrl { get; set; }
    }
}
