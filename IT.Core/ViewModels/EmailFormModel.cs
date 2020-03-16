using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class EmailFormModel
    {
        [Required, Display(Name = "Your name")]
        public string FromName { get; set; }

        //[Required, Display(Name = "From email"), EmailAddress]
        // public string FromEmail { get; set; }

        public string Message { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string CC { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string ToEmail { get; set; }


        public string LPOINvoiceId { get; set; }
        public string FileName { get; set; }

        public string FlagToRedirect { get; set; }
    }
}
