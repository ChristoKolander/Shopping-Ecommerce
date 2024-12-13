namespace Shopping.Api.CQRS.Queries.Query
{
 
    public class QueryResult<T> where T : class
    {
    
        public bool Successful { get; set; } = false;
        public string Error { get; set; } = null;
        public T Item { get; set; } = null;
        public List<T> Items { get; set; } = null;
    }
}
