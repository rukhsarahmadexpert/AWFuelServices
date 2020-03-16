using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class ExpenseViewModel
    {
        public int Id { get; set; }
        public string ExpenseNumber { get; set; }
        public int EmployeeId { get; set; }
        public string Category { get; set; }
        public int ItemRefrenceId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int ApproveBy { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal IssuedAmount { get; set; }
        public int PaymentTerm { get; set; }
        public bool IsActive { get; set; }
        public decimal Total { get; set; }
        public decimal VAT { get; set; }
        public decimal GrandTotal { get; set; }
        public int detailId { get; set; }
        public string ExpensePadNumber { get; set; }
        public string UserName { get; set; }
        public string EmployeeName { get; set; }
        public string CreatedDates { get; set; }

        public List<ExpenseDetailsViewModel> expenseDetailsList { get; set; }
    }
}
