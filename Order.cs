using System;
using Newtonsoft.Json;

namespace ServiceBus
{
    public class Order
    {
        public Guid OrderID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
        
    }

}