using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class BookingUpdateReason
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string UpdateReason { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
    }
}
	