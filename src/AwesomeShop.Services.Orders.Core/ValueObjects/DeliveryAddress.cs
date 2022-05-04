using System;

namespace AwesomeShop.Services.Orders.Core.ValueObjects
{
    public class DeliveryAddress
    {
        public string Steet { get; private set; }
        public string Number { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string ZipCode { get; private set; }

        public DeliveryAddress(string street, string number, string city, string state, string zipCode)
        {
            Steet = street;
            Number = number;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public override bool Equals(object obj)
        {
            return obj is DeliveryAddress address &&
                   Steet == address.Steet &&
                   Number == address.Number &&
                   City == address.City &&
                   State == address.State &&
                   ZipCode == address.ZipCode;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Steet, Number, City, State, ZipCode);
        }
    }
}
