using Shopping.Web.Portal.Features.RequestFeatures;


namespace Shopping.Web.Portal.Features.PagingFeatures
{
    public class PagingResponse<T> where T : class
    {
        public List<T> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
