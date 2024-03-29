﻿namespace Shopping.Web.Portal.Features.RequestFeatures
{
    public class QueryStringParameters
    {
        public string SearchTerm { get; set; } = "";

        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 5;
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
