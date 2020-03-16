using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class WebSiteImagesviewModel
    {
        public int Id { get; set; }
        public string WebImageUrl { get; set; }
        public int AboutUsId { get; set; }
        public int HomeId { get; set; }
        public int ContactUsId { get; set; }
        public int ServicesId { get; set; }
        public int PrivacyPolicyId { get; set; }
        public int TermConditionId { get; set; }
        public int FrequentlyAskQuestionId { get; set; }
        public int CreatedBy { get; set; }      
        public int UpdatedBy { get; set; }
        public int IsActive { get; set; }
    }
}

       
	  