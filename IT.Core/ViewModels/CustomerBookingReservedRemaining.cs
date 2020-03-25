using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class CustomerBookingReservedRemaining
    {
       public List<CustomerBookingViewModel> Reserved { get; set; }
       public List<CustomerBookingViewModel> Lefted { get; set; }
       public List<CustomerBookingViewModel> Remaining { get; set; }
       public List<CustomerBookingViewModel> Pending { get; set; }
       public CustomerBookingViewModel MostRecentBooking { get; set; }
       public BookLeftRemainingViewModel bookLeftRemainingViewModel { get; set; }
    }
}
