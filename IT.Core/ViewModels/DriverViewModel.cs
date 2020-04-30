using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class DriverViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string Comments { get; set; }
        public string PassportCopy { get; set; }
        public string VisaCopy { get; set; }
        public string IDUAECopyFront { get; set; }
        public string IDUAECopyBack { get; set; }
        public string DrivingLicenseFront { get; set; }
        public string DrivingLicenseBack { get; set; }
        public int? LicenseType { get; set; }
        public int? LicenseType2 { get; set; }
        public int? LicenseType3 { get; set; }
        public string Nationality { get; set; }
        public string DrivingLicenseExpiryDate { get; set; }
        public string LicenseExpiry { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string UID { get; set; }
        public string UserName { get; set; }
        public int DriverLoginId { get; set; }
        public int VehicleId { get; set; }
        public string DriverImageUrl { get; set; }
        public string PassportBack { get; set; }

        public string[] LicienceList { get; set; }
        public List<int> LicenseTypes { get; set; }

        public int TotalRows { get; set; }

        public List<UploadDocumentsViewModel> uploadDocumentsViewModels { get; set; }
    }
}
