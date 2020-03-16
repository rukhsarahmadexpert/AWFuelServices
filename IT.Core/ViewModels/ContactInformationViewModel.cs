using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class ContactInformationViewModel
    {
        public int Id { get; set; }
        public string TitleOne { get; set; }
        public string TitleTwo { get; set; }
        public string Address { get; set; }
        public string Emails { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }

        public List<WebSiteImagesviewModel> webSiteImagesviewModelsContacts { get; set; }
    }
}

