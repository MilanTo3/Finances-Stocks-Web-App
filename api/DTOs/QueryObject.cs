using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class QueryObject
    {
        public string? Symbol{get; set;} = null;
        public string? CompanyName{get; set;} = null;
        public string? SortBy {get;set;} = null;
        public bool ascending {get;set;} = false;
        public int pageNumber{get;set;} = 1;
        public int pageSize{get;set;} = 20;
    }
}