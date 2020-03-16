using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class AWFuelSalaryViewModel
    {
        public int Id { get; set; }
        public string VoucharNo { get; set; }
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }
        public decimal Receivable { get; set; }
        public decimal Received { get; set; }
        public int IssuedBy { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime IssuedDate { get; set; }        
        public string PaymentType { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdateReason { get; set; }
        public decimal Deduction { get; set; }
        public decimal LoanIssued { get; set; }
        public decimal LoanReturn { get; set; }
        public decimal Allowance { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Name { get; set; }
    }
}



	
	