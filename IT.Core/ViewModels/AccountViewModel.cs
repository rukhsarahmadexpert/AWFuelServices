using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int VenderId { get; set; }
        public decimal Received { get; set; }
        public decimal Paid { get; set; }
        public int PaymentTerm { get; set; }
        public string PaymentTerms { get; set; }
        public string PayedPersonName { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string CheckNumber { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsCleared { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string CreatedDates { get; set; }
        public string Currency { get; set; }
        public string Vouchar { get; set; }
        public string PostedDates { get; set; }
        public string Description { get; set; }
        public string SalayYear { get; set; }
        public string SalaryMonth { get; set; }

        public List<AccountDetailsViewModel> accountDetailsViewModels { get; set; }
    }
}
