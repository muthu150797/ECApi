using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcAPI.Entity.OrderAggregrate
{
    public class OrderItem:BaseEntity
    {
        public OrderItem()
        {
        }

        public OrderItem(ProductItmeOrdered itmeOrdered, decimal price, int quantity)
        {
            ItmeOrdered = itmeOrdered;
            Price = price;
            Quantity = quantity;
        }
        //public int OrderId { get; set; }    
        public ProductItmeOrdered ItmeOrdered { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
    }
}