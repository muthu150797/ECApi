using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcAPI.Entity.OrderAggregrate
{
    public class Order:BaseEntity
    {
        public Order()
        {
        }

        public Order(IReadOnlyList<OrderItem> orderItems,string buyerEmail, Address shipToAddress, DeliveryMethod deliveryMethod,  decimal subtotal, string paymentId)
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            Subtotal = subtotal;
            PaymentId = paymentId;
            PaymentIntentId = paymentId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public Address ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        //after update dp start
        public int DeliveryMethodId { get; set; }
        //public DeliveryMethod DeliveryMethods { get;set; }  

        //end
         public OrderStatus Status { get; set; }
        // public string Status { get; set; }

        public string PaymentIntentId { get; set; }
        public string PaymentId { get; set; }
        public decimal GetTotal()
        {
          return Subtotal+DeliveryMethod.Price;
        }
    }
}