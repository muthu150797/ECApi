using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcAPI.Entity.OrderAggregrate;

namespace EcAPI.Entities
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public Address ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal ShippingPrice { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }

    }
}