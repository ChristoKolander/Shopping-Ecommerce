﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Api.Entities.RequestFeatures
{
    public class QueryStringParameters
    {

        public string SearchTerm { get; set; }

        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 4;
        public int PageSize
        {
            get
            {
                return pageSize;

            }
            set
            {
                pageSize = (value > maxPageSize) ? maxPageSize : value;

            }
        }
       
    }
}
