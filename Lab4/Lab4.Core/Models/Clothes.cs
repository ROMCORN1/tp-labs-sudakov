using System;

namespace Lab4.Core.Models
{
    public class Clothes : Product
    {
        private string _size = string.Empty;

        public string Size
        {
            get { return _size; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Размер не может быть пустым.", nameof(value));
                _size = value;
            }
        }

        public Clothes(string name, double price, string size, double discount = 0)
            : base(name, price, discount)
        {
            Size = size;
        }

        public override string Category => "Одежда";

        public override double GetFinalPrice()
        {
            double price = base.GetFinalPrice();
            if (string.Equals(Size, "XL", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(Size, "XXL", StringComparison.OrdinalIgnoreCase))
                price *= 0.9;
            return price;
        }

        public override string ToString() => base.ToString() + " | Размер: " + Size;
    }
}