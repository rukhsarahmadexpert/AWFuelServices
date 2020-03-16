using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class RejectedOrderDetails
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
        public string CreatedDates { get; set; }
    }
}
