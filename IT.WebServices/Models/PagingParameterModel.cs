using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT.WebServices.Models
{
    public class PagingParameterModel
    {
        const int maxPageSize = 40;

        public int pageNumber { get; set; } = 1;
        public int _pageSize { get; set; } = 10;
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string OrderProgress { get; set; }
        public bool IsSend { get; set; }
        public string SerachKey { get; set; }
        public string SearchFlag { get; set; }
        public int DriverId { get; set; }
        public int pageSize
        {

            get { return _pageSize; }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}