using System.Runtime.Serialization;

namespace EcAPI.Entity.OrderAggregrate
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Payment Received")]
        PaymentReceived,
        [EnumMember(Value = "Payment Failed")]
        PendingFailed
    }
}