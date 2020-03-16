using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class ExpenseDetailsViewModel
    {
        public int Id { get; set; }
        public int ExpenseId { get; set; }
        public string Description { get; set; }
        public int ExpenseType { get; set; }
        public decimal SubTotal { get; set; }
        public decimal VAT { get; set; }
        public decimal NetTotal { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OnDates { get; set; }
        public string ExpDates { get; set; }
        public string Category { get; set; }
        public int ExpenseRefrenceId { get; set; }
        public string TraficPlateNumber { get; set; }
        public string ExpenseName { get; set; }
        public string Name { get; set; }
    }
}
