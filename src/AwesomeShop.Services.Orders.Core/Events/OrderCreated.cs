using AwesomeShop.Services.Orders.Core.ValueObjects;
using System;

namespace AwesomeShop.Services.Orders.Core.Events
{
    public class OrderCreated : IDomainEvent
    {
        public Guid Id { get; private set; }
        public decimal TotalPrice { get; private set; }
        public PaymentInfo PaymentInfo { get; private set; }
        public string CustomerFullName { get; private set; }
        public string CustomerEmail { get; private set; }

        public OrderCreated(Guid id, decimal totalPrice, PaymentInfo paymentInfo, string customerFullName, string customerEmail)
        {
            Id = id;
            TotalPrice = totalPrice;
            PaymentInfo = paymentInfo;
            CustomerFullName = customerFullName;
            CustomerEmail = customerEmail;
        }
    }
}
