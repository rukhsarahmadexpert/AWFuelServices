using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class RolesViewModel
    {
        public int AuthorityId { get; set; }
        public string Authority { get; set; }
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }
    }
}
