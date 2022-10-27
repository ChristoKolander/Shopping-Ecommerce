using System;

namespace Shopping.Core.Entities.CQRSresponses
{
    public class QueryResponse
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string OrderName { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderItems { get; set; }
        public int TotalCost { get; set; }

    }
}

