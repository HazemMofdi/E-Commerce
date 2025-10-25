using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ProductSpecificationParameters
    {
        
        public int? TypeId { get; set; }
        public int? BrandId { get; set; }
        public ProducsSortingOptions Sort { get; set; }
        public string? Search{ get; set; }
        ///////

        private const int defaultPageSize = 5;
        private const int maxPageSize = 10;
        public int PageIndex { get; set; } = 1;
        //////
        private int _pageSize = defaultPageSize;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize =  value >  maxPageSize? maxPageSize: value; }
        }


    }
}
