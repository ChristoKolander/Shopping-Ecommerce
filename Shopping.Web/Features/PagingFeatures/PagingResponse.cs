using Shopping.Web.Features.RequestFeatures;
using System.Collections.Generic;


namespace Shopping.Web.Features.PagingFeatures
{
    public class PagingResponse<T> where T : class
    {
        public List<T> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
