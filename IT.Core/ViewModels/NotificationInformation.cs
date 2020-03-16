using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class NotificationInformation
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string DeviceToken { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Email { get; set; }
        public string Authority { get; set; }
        public string Device { get; set; }

    }
}


