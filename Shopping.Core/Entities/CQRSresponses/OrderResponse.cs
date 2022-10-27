using System;

namespace Shopping.Core.Entities.CQRSresponses
{
    
    public class OrderResponse
    { 

    public int Id { get; set; }
    public string UserEmail { get; set; }
    public string OrderName { get; set; }
    public DateTime OrderDate { get; set; }
    public string Products { get; set; }
    public int TotalCost { get; set; }

           
    }
}
    

